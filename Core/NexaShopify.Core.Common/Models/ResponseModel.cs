namespace NexaShopify.Core.Common.Models
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; } = false;
        public List<ResponseError> Errors { get; set; } = new List<ResponseError>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Infos { get; set; } = new List<string>();
        public T Body { get; set; }

   

        public ResponseModel()
        { }
        public ResponseModel(T body)
        {
            this.Body = body;
        }

        // >>> 05
        public static ResponseModel<T> SuccessResponse()
        {
            return new ResponseModel<T>()
            {
                Success = true
            };
        }
        public static ResponseModel<T> SuccessResponse(T body)
        {
            return new ResponseModel<T>()
            {
                Success = true,
                Body = body
            };
        }
        public static ResponseModel<T> SuccessResponse(T body, List<string> warnings) 
        {
            return new ResponseModel<T>()
            {
                Success = true,
                Body = body,
                Warnings = warnings,
            };
        }

        public static ResponseModel<T> UnexpectedErrorResponse()
        {
            return new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
                {
                new ResponseError() { Key = "GNR0001", Value = "An unexpected error has occurred" } }
            };
        }
        public static ResponseModel<T> AccessDeniedResponse()
        {
            return new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
                {
                new ResponseError() { Key = "GNR0002", Value = "Access denied" } }
            };
        }

        public static ResponseModel<T> NotFoundResponse()
        {
            return new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0003", Value = "Item not found" } }
            };
        }
        public static ResponseModel<T> WrongPasswordResponse()
        {
            return new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0004", Value = "Wrong password" } }
            };
        }
        public static ResponseModel<T> ExistItemResponse()
        {
            return new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0005", Value = "Exists" } }
            };
        }
        public static ResponseModel<T> FailureResponse(string value)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Errors = new List<ResponseError>
                {
                    new ResponseError
                    {
                        Key = "",
                        Value = value
                    }
                }
            };
        }
        public static ResponseModel<T> FailureResponse(string key, string value)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Errors = new List<ResponseError> // ==> this is a List here 
				{
                    new ResponseError  // ===> it's an item or item of List
					{
                        Key = key,
                        Value = value
                    }
                }
            };
        }
        public static ResponseModel<T> FailureResponse(List<string> values)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Errors = values?.Select(x => new ResponseError { Key = x, Value = x })?.ToList()
            };
        }
        public static ResponseModel<T> FailureResponse(List<string> keys, List<string> values)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Errors = keys?.Select((x, i) => new ResponseError { Key = x, Value = values[i] })?.ToList()

            };
        }
        public static ResponseModel<T> FailureResponse(List<KeyValuePair<string, string>> values)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Errors = values?.Select(x => new ResponseError { Key = x.Key, Value = x.Value })?.ToList()

            };
        }

        #region Async
        public async static Task<ResponseModel<T>> SuccessResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Success = true
            });
        }
        public async static Task<ResponseModel<T>> SuccessResponseAsync(T body)
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Success = true,
                Body = body
            });
        }

        public async static Task<ResponseModel<T>> UnexpectedErrorResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0001", Value = "An unexpected error has occurred" } }
            });
        }
        public async static Task<ResponseModel<T>> AccessDeniedResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0002", Value = "Access denied" } }
            });
        }
        public async static Task<ResponseModel<T>> NotFoundResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0003", Value = "Item not found" } }
            });
        }
        public async static Task<ResponseModel<T>> WrongPasswordResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0004", Value = "Wrong password" } }
            });
        }
        public async static Task<ResponseModel<T>> ExistItemResponseAsync()
        {
            return await Task.FromResult(new ResponseModel<T>()
            {
                Errors = new List<ResponseError>()
            {
                new ResponseError() { Key = "GNR0005", Value = "Exists" } }
            });
        }
        public async static Task<ResponseModel<T>> FailureResponseAsync(string value)
        {
            return await Task.FromResult(new ResponseModel<T>
            {
                Success = false,
                Errors = new List<ResponseError>
                {
                    new ResponseError
                    {
                        Key = "",
                        Value = value
                    }
                }
            });
        }
        public async static Task<ResponseModel<T>> FailureResponseAsync(string key, string value)
        {
            return await Task.FromResult(new ResponseModel<T>
            {
                Success = false,
                Errors = new List<ResponseError>
                {
                    new ResponseError
                    {
                        Key = key,
                        Value = value
                    }
                }
            });
        }
        public async static Task<ResponseModel<T>> FailureResponseAsync(List<string> values)
        {
            return await Task.FromResult(new ResponseModel<T>
            {
                Success = false,
                Errors = values?.Select(x => new ResponseError { Key = x, Value = x })?.ToList()
            });
        }
        public async static Task<ResponseModel<T>> FailureResponseAsync(List<string> keys, List<string> values)
        {
            return await Task.FromResult(new ResponseModel<T>
            {
                Success = false,
                Errors = keys?.Select((x, i) => new ResponseError { Key = x, Value = values[i] })?.ToList()

            });
        }
        public async static Task<ResponseModel<T>> FailureResponseAsync(List<KeyValuePair<string, string>> values)
        {
            return await Task.FromResult(new ResponseModel<T>
            {
                Success = false,
                Errors = values?.Select(x => new ResponseError { Key = x.Key, Value = x.Value })?.ToList()

            });
        }

        #endregion Async

        public class ResponseError
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public ResponseError() { }
            public ResponseError(string value)
            {
                Value = value;
            }
            public ResponseError(KeyValuePair<string, string> val)
            {
                Key = val.Key;
                Value = val.Value;
            }
        }
    }
}


