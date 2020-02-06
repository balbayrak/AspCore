using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.General
{
    public class BusinessConstants
    {
        public struct BaseExceptionMessages
        {
            public const string TASK_EXCEPTION = "Akış İşleminde hata Oluştu!, Sistem yöneticisine haber veriniz!";
            public const string TASK_INFO_NOT_FOUND = "Task bilgileri boş olamaz!";
            public const string TASK_USER_INFO_NOT_FOUND = "Request içerisinde aktif kullanici bilgisi alınamadı!";
            public const string PARAMETER_IS_NULL = "{0} is null";
            public const string PARAMETER_IS_GUID_EMPTY = "{0} is guid empty";
            public const string PARAMETER_IS_NULL_OR_EMPTY = "{0} is null or empty";
            public const string MODEL_INVALID = "Invalid model object";
            public const string TASK_ACTION_EXCEPTION = "Task içerisinde action bulunamadı!, Controller action ismi ile task içerisinde aynı isimde action bulunmalıdır!";
            public const string TASK_ACTION_RUN_EXCEPTION = "Task içerisinde action çalıştırılamadı!";
        }

        public struct MiddlewareErrorMessages
        {
            public const string INTERNAL_SERVER_ERROR_OCCURRED = "Internal Server Error from the custom middleware!";
        }

        public struct DocumentUploaderErrorMessages
        {
            public const string DOCUMENT_CREATE_METHOD_ERROR = "Document oluşturma kodlarında hata oluştu! Sistem yöneticisine haber veriniz!";
            public const string DOCUMENT_READ_METHOD_ERROR = "Document okuma kodlarında hata oluştu! Sistem yöneticisine haber veriniz!";
            public const string DOCUMENT_DELETE_METHOD_ERROR = "Document silme kodlarında hata oluştu! Sistem yöneticisine haber veriniz!";
            public const string DOCUMENT_UPDATE_METHOD_ERROR = "Document güncelleme kodlarında hata oluştu! Sistem yöneticisine haber veriniz!";
            public const string DOCUMENT_DELETE_AFTER_DATAACCESS_METHOD_ERROR = "DataAccess kodlarında hata oluştu, Oluşturulan dokuman silinemedi! Sistem yöneticisine haber veriniz!";
        }

        public struct JWT_Error_Messages
        {
            public const string BEARER_TOKEN_NOT_FOUND = "Request header içerisinde bearer token bilgisi bulunamadı!";
            public const string BEARER_TOKEN_GET_ERROR = "Request header içerisinde bearer token bilgisi alınırken hata oluştu!";
        }

    }
}
