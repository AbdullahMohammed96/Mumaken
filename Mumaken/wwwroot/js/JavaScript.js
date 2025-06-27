function postJson() {

    var addObj = {
        fkHeaderAds: 113,
        fkRegoin: 0,
        fkCity: 0,
        fkDistrict: 1,
        fkCat: 0,
        title: '',
        lat: '',
        lng: '',
        location: '',
        determinePrice: false,
        elMamsha: 0,
        showPhone: false,
        phone: '',
        price: '',
        closeReply: false,
        description: '',
        FromAppOrNo: false,
        lang: 'ar',
    };


    var data = new FormData();
    for (var i = 0; i < images.length; i++) {
        data.append("imgs", images[i].file);
    }

    $.each(addObj, function (key, val) {
        data.append(key, val);
    });

    var fd = new FormData();

    fd.append('user_img', $('#image').get(0).files[0]);

    fd.append('user_address', $('#Address').val());
    fd.append('user_lat', $('#Lat').val());
    fd.append('user_lng', $('#Lng').val());

    fd.append('user_name', $('#name').val());
    fd.append('user_phone', $('#phone').val());
    fd.append('user_email', $('#email').val());
    fd.append('user_Password', $('#password').val());
    fd.append('lang', "ar");

    $.ajax({
        url: "@Url.Action("Register","SClient")",
        type: "Post",
        data: fd,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.key == 1) {
                toastr.success(data.msg);
                setTimeout(function () {
                    location.assign("@Url.Action("ActiveCode","SClient")" + "?user_id=" + data.id);
                }, 500);
            } else {
                toastr.error(data.msg);
            }
        }
    });

}