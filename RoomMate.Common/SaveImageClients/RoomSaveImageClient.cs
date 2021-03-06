﻿using RoomMate.Data.Context;
using RoomMate.Data.UnitOfWorks;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RoomMate.Common.SaveImageClients
{
    public class RoomSaveImageClient : SaveImageClient
    {
        private RoomImage roomImage;
        private List<RoomImage> roomImagesList;
        public RoomSaveImageClient()
        {
            roomImagesList = new List<RoomImage>();
        }
        public List<RoomImage> saveRoomImageToDiskAndReturnListWithRoomImages(IEnumerable<HttpPostedFileBase> roomImages, Guid roomID, Guid userID, Room room)
        {
            string directoryPath = "~/Content/IMAGE/" + userID + "/" + roomID + "/";

            createDirectory(directoryPath);
            deleteFilesFromDirectory(directoryPath);

            try
            {
                foreach (HttpPostedFileBase file in roomImages)
                {
                    if (file != null)
                    {
                        string pathToImage = directoryPath + Path.GetFileName(file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath(pathToImage));
                        createRoomImageObjectAndAddToList(pathToImage, room);
                    }
                    else
                    {
                        string pathToImage = "~/Content/IMAGE/no-image.PNG";
                        createRoomImageObjectAndAddToList(pathToImage, room);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                throw e;
            }

            return roomImagesList;
        }

        private void createRoomImageObjectAndAddToList(string pathToImage, Room room)
        {
            roomImage = new RoomImage();

            roomImage.ImageRoomID = Guid.NewGuid();
            roomImage.Path = pathToImage;
            roomImage.Room = room;
            roomImagesList.Add(roomImage);
        }

    }
}