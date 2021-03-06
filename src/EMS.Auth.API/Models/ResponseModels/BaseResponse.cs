﻿using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class BaseResponse
    {
        public bool IsSucess { get; set; }
        public string ErrorMessage { get; set; }
        public long Id { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not BaseResponse)
            {
                return false;
            }
            BaseResponse toCompare = obj as BaseResponse;
            return IsSucess == toCompare.IsSucess
                && ErrorMessage == toCompare.ErrorMessage
                && Id == toCompare.Id;
        }
    }
}
