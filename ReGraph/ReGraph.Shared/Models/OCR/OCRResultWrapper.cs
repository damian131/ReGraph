using ReGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.Models.OCR
{
    public class OCRResultWrapper
    {
		public OCRResultWrapper(String Result, OCRTypeResult ResultType)
		{
			this.Result = Result;
			this.ResultType = ResultType;
		}

		public String Result { get; private set; }
		public OCRTypeResult ResultType { get; private set; }
    }
}
