var Loader = {
    loaderStart: function (data) {
        // $("#loadSpin").show();
        $(".big-loader").show();
        $("select").select2();
        $.blockUI();
  
    },
    loaderFinish: function () {
        // $("#loadSpin").hide();
        $(".big-loader").hide();
        $(".tooltipShow").tooltip();
        $("select").select2();
        $.unblockUI();
    },
    AjaxTemplate: '<p class="padding-20"><i class="fa fa-spinner fa-spin"></i> İçerik yüklenirken lütfen bekleyiniz...</p>',
    AjaxSaveTemplate: '<i class="fa fa-spinner fa-spin"></i> İçerik kaydedilirken lütfen bekleyiniz...',

}


$(window).on('beforeunload', function () {
    Loader.loaderStart();
});

function StartIt() {
    $(window).on('beforeunload', function () {
        Loader.loaderStart();
    });
    return true;
}

function StopIt() {
    $(window).off('beforeunload');
    Loader.loaderFinish();
    return true;
}

$(document).ajaxStart(function (e) {
    Loader.loaderStart();
});

$(document).ajaxStop(function (e) {
    Loader.loaderFinish();
});

$(document).ajaxError(function () {
    toastr["error"]("Sistemde bir hata oluştu! Daha sonra tekrar deneyiniz.");
});

Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};


$(document).keyup(function (e) {
    if (e.keyCode === 27) {
        Loader.loaderFinish();
        $.unblockUI();
        $(".tooltipShow").tooltip();
    }
});


var n = 600;
var oturumTimeout = setTimeout(countDown, 1000);
var oturumConfirm = true;
function countDown() {
    n--;
    if (n > 0) {
        oturumTimeout = setTimeout(countDown, 1000);
        if (n < 60) {
            if (oturumConfirm) {
                bootbox.confirm("Oturumunuz sonlandırılmak üzere, devam etmek istiyor musunuz?", function (result) {
                    if (result) {
                        clearTimeout(oturumTimeout);
                        n = 600;
                        oturumConfirm = true;
                        oturumTimeout = setTimeout(countDown, 1000);
                    } else {
                        window.location.href = '/Account/Logout';
                    }
                });
                oturumConfirm = false;
            }
        }
    } else {
        window.location.href = '/Account/Logout';
    }
}


$(document).ready(function () {
    //$(".picker").datetimepicker({
    //    locale: 'tr',
    //    format: 'YYYY-MM-DD',
    //});

    $.ajaxSetup({ cache: false });

    //$(".picker1").datetimepicker({
    //    locale: 'tr',
    //    format: 'YYYY-MM-DD',
    //    minDate: new Date(),
    //    useCurrent: false
    //});

    $("body").delegate(".toast", "click", function (e) {
        e.preventDefault();
        $(".toast").hide();
    });

    $("select").select2();

    if (window.location.search.search("Ticket") > 0) {
        window.history.pushState({}, document.title, "/");
    }



    $(document.body).on('hidden.bs.modal', function () {
        $('.modal').removeData('bs.modal')
    });


});
