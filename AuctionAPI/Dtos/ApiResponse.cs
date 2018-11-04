using System;
using System.Collections.Generic;

namespace AuctionAPI.Dtos
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Message = String.Empty;
            ErrorMessages = new List<string>();
        }

        public bool Success
        {
            get { return ErrorMessages.Count == 0; }
        }

        public string Message { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}