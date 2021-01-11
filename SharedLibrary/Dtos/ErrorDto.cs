using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; }
        public bool isShow { get; private set; }
        public ErrorDto()
        {
            Errors = new List<string>();
        }
        public ErrorDto(string error,bool _isShow)
        {
            isShow = _isShow;
            Errors.Add(error);
        }
        public ErrorDto(List<string> errors, bool _isShow)
        {
            isShow = _isShow;
            Errors = errors;
        }
    }
}
