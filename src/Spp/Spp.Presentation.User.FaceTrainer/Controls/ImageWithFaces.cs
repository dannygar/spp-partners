/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Windows.Storage;

namespace FaceAPITrainer.Controls
{
    public class ImageWithFace
    {
        private static FaceAttributeType[] DefaultFaceAttributeTypes = new FaceAttributeType[] { FaceAttributeType.Age, FaceAttributeType.Gender };

        public event EventHandler FaceDetectionCompleted;
        public event EventHandler FaceRecognitionCompleted;

        public StorageFile LocalImageFile { get; set; }
        public string ImageUrl { get; set; }

        public IEnumerable<Face> DetectedFaces { get; set; }

        public IEnumerable<IdentifiedPerson> IdentifiedPersons { get; set; }

        // Default to no faces detected since this could trigger a stream of popup errors since we call this
        // for several images at once while auto-detecting the Bing Image Search results.
        public bool ShowDialogOnFaceApiErrors { get; set; } = false;

        public ImageWithFace(string url)
        {
            this.ImageUrl = url;
        }

        public ImageWithFace(StorageFile localFile)
        {
            this.LocalImageFile = localFile;
        }

        public async Task DetectFacesAsync(bool detectFaceAttributes = false, int imageWidth = 0, int imageHeight = 0)
        {
            try
            {
                if (this.ImageUrl != null)
                {
                    this.DetectedFaces = await FaceServiceHelper.DetectAsync(
                        this.ImageUrl,
                        returnFaceId: true,
                        returnFaceLandmarks: false,
                        returnFaceAttributes: detectFaceAttributes ? DefaultFaceAttributeTypes : null);
                }
                else if (this.LocalImageFile != null)
                {
                    this.DetectedFaces = await FaceServiceHelper.DetectAsync(
                        await this.LocalImageFile.OpenStreamForReadAsync(),
                        returnFaceId: true,
                        returnFaceLandmarks: false,
                        returnFaceAttributes: detectFaceAttributes ? DefaultFaceAttributeTypes : null);
                }
            }
            catch (Exception e)
            {
                this.DetectedFaces = Enumerable.Empty<Face>();

                if (this.ShowDialogOnFaceApiErrors)
                {
                    await Util.GenericApiCallExceptionHandler(e, "Face detection failed.");
                }
            }
            finally
            {
                this.OnFaceDetectionCompleted();
            }
        }
        public async Task IdentifyFacesAsync()
        {
            this.IdentifiedPersons = Enumerable.Empty<IdentifiedPerson>();

            try
            {
                Guid[] detectedFaceIds = this.DetectedFaces.Select(f => f.FaceId).ToArray();
                if (detectedFaceIds.Any())
                {
                    List<IdentifiedPerson> result = new List<IdentifiedPerson>();

                    var key = string.IsNullOrWhiteSpace(SettingsHelper.Instance.WorkspaceKey)
                                  ? null
                                  : SettingsHelper.Instance.WorkspaceKey;

                    IEnumerable<PersonGroup> personGroups = await FaceServiceHelper.GetPersonGroupsAsync(key);
                    foreach (var group in personGroups)
                    {
                        IdentifyResult[] groupResults = await FaceServiceHelper.IdentifyAsync(group.PersonGroupId, detectedFaceIds);
                        foreach (var match in groupResults)
                        {
                            if (!match.Candidates.Any())
                            {
                                continue;
                            }

                            Person person = await FaceServiceHelper.GetPersonAsync(group.PersonGroupId, match.Candidates[0].PersonId);

                            IdentifiedPerson alreadyIdentifiedPerson = result.FirstOrDefault(p => p.Person.PersonId == match.Candidates[0].PersonId);
                            if (alreadyIdentifiedPerson != null)
                            {
                                // We already tagged this person in another group. Replace the existing one if this new one if the confidence is higher.
                                if (alreadyIdentifiedPerson.Confidence < match.Candidates[0].Confidence)
                                {
                                    alreadyIdentifiedPerson.Person = person;
                                    alreadyIdentifiedPerson.Confidence = match.Candidates[0].Confidence;
                                    alreadyIdentifiedPerson.FaceId = match.FaceId;
                                }
                            }
                            else
                            {
                                result.Add(new IdentifiedPerson { Person = person, Confidence = match.Candidates[0].Confidence, FaceId = match.FaceId });
                            }
                        }
                    }

                    this.IdentifiedPersons = result;
                }
            }
            catch (Exception e)
            {
                if (this.ShowDialogOnFaceApiErrors)
                {
                    await Util.GenericApiCallExceptionHandler(e, "Failure identifying faces");
                }
            }
            finally
            {
                this.OnFaceRecognitionCompleted();
            }
        }

        private void OnFaceDetectionCompleted()
        {
            if (this.FaceDetectionCompleted != null)
            {
                this.FaceDetectionCompleted(this, EventArgs.Empty);
            }
        }

        private void OnFaceRecognitionCompleted()
        {
            if (this.FaceRecognitionCompleted != null)
            {
                this.FaceRecognitionCompleted(this, EventArgs.Empty);
            }
        }
    }
}
