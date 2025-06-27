// loader js
$(window).on("load", function () {
  $(".loader-container").delay(300).fadeOut(1000);
});


// Show And Hide SlideBar
const menu = document.getElementById("menu");
const sideBar = document.getElementById("sidebar");
const navbar = document.getElementById("navbar");
const main = document.getElementById("main");
const closeSidebar = document.getElementById("closeSidebar");
let isRtl = $('html[lang="ar"]').length > 0;

if (menu) {
  menu.addEventListener("click", () => {
    sideBar.classList.toggle("active-side");
    navbar.classList.toggle("active-nav");
    main.classList.toggle("active-main");
  });
}

// Close SideBar
if (closeSidebar) {
  closeSidebar.addEventListener("click", () => {
    sideBar.classList.remove("active-side");
    navbar.classList.remove("active-nav");
    main.classList.remove("active-main");
  });
}

$(".sidebar .links li a").each(function () {
  if (window.location.href.includes($(this).attr("href"))) {
    $(this).addClass("active");
  }
});

// PassWord Show In Setting Page
const iconsPassSet = document.querySelectorAll(".pass-icon");

if (iconsPassSet) {
  iconsPassSet.forEach((ic) => {
    ic.addEventListener("click", function () {
      let input = ic.parentElement.querySelector(".input-me");
      showPassword(input, ic);
    });
  });
}

// Function To Show And Hide Password
function showPassword(input, icon) {
  if (input.type == "password") {
    input.setAttribute("type", "text");
    icon.innerHTML = `<i class="fa-regular fa-eye-slash"></i>`;
  } else {
    input.setAttribute("type", "password");
    icon.innerHTML = `<i class="fa-regular fa-eye"></i>`;
  }
}

//================================ DataTable ============================//
$(document).ready(function () {
  let tableTanguage = {};

  let arTable = {
    paginate: {
      previous: `<i class="fa-solid fa-angles-left"></i>`,
      next: `<i class="fa-solid fa-angles-right"></i>`,
    },
    sProcessing: "جارٍ التحميل...",
    sLengthMenu: "أظهر _MENU_ مدخلات",
    sZeroRecords: "لم يعثر على أية سجلات",
    sInfo: "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
    sInfoEmpty: "يعرض 0 إلى 0 من أصل 0 سجل",
    sInfoFiltered: "(منتقاة من مجموع _MAX_ مُدخل)",
    sInfoPostFix: "",
  };

  let enTable = {
    paginate: {
      previous: `<i class="fa-solid fa-angles-left"></i>`,
      next: `<i class="fa-solid fa-angles-right"></i>`,
    },
    sLengthMenu: "Display _MENU_ records per page",
    sZeroRecords: "Nothing found - sorry",
    zInfo: "Showing page _PAGE_ of _PAGES_",
    sInfoEmpty: "No records available",
    sInfoFiltered: "(filtered from _MAX_ total records)",
  };

  if (isRtl) {
    tableTanguage = arTable;
  } else {
    tableTanguage = enTable;
  }

  if ($("#myTable").length > 0) {
    var myTable = $("#myTable").dataTable({
      pageLength: 7,
      // responsive: true,
      bLengthChange: false,
      language: tableTanguage,
      ordering: true,
    });

    $("#searchTable").on("keyup", function () {
      $("#myTable").DataTable().search($(this).val()).draw();
    });
  }
});

//================================ Upload Files Or Img ============================//
let loginInputs = document.querySelectorAll(".img-upload-input");

loginInputs.forEach((input) => {
  if (input.classList.contains("profile")) {
    const profileCon = input.closest(".profile-img");
    const img = profileCon.querySelector("img");

    input.onchange = function () {
      let reader = new FileReader();
      if (input.files[0]) {
        reader.readAsDataURL(input.files[0]);
      } else {
        console.log("no file");
      }

      reader.onload = () => {
        img.src = reader.result;
      };
    };
  } else {
    input.addEventListener("change", function () {
      imgPreview(input);
    });
  }
});

// ImgPreview Function
function imgPreview(input) {
  var file = input.files[0];
  var mixedfile = file["type"].split("/");
  var filetype = mixedfile[0];
  let photoContainer = $(input).closest(".upload-con").find(".photo-con");

  const multiple = $(input).attr("multiple");

  if (multiple) {
    if (filetype == "image") {
      uploadMultiImgs(input, photoContainer);
    } else if (filetype == "application") {
      uploadFile(input, photoContainer);
    }
  } else {
    if (filetype == "image") {
      uploadImg(input, photoContainer);
    } else if (filetype == "application") {
      photoContainer.empty();
      uploadFile(input, photoContainer);
    } else {
      alert("Invalid file type");
    }
  }
}

// uploadMultiImgs
function uploadMultiImgs(input, photoContainer) {
  for (file of input.files) {
    let reader = new FileReader();
    reader.onload = () => {
      let img = `
          <div class="hidden-img">
              <input type='hidden' value='${reader.result}' />
              <a class="fancybox" data-fancybox="gallery" href="${reader.result}">
                  <img class="img" src="${reader.result}" />
              </a>

              <button type='button' class='remove-img'>
                  <i class="fa-solid fa-xmark"></i>
              </button>

          </div>
      `;

      photoContainer.append(img);
      removeIcon();
    };
    reader.readAsDataURL(file);
  }
}

// Upload Image
function uploadImg(input, photoContainer) {
  var reader = new FileReader();
  reader.onload = function (e) {
    photoContainer.empty();
    let img = `
        <div class="hidden-img">

            <a class="fancybox" data-fancybox="gallery" href="${e.target.result}">
                <img class="img" src="${e.target.result}" />
            </a>

            <button type='button' class='remove-img'>
                <i class="fa-solid fa-xmark"></i>
            </button>

        </div>
    `;

    photoContainer.append(img);
    removeIcon();
  };
  reader.readAsDataURL(input.files[0]);
}

// uploadFiles
function uploadFile(input, photoContainer) {
  Object.values(input.files).forEach(function (file) {
    var name = file.name;

    let myFile = `
          <div class="upload-label mt-3">
              <input type='hidden' value='${name}' />
              <i class="fa-regular fa-file ic"></i>
              <span>${name}</span>
              <button type='button' class='remove-img'>
                <i class="fa-solid fa-xmark"></i>
            </button>
          </div>
        `;

    photoContainer.append(myFile);
    removeIcon();
  });
}

// Remove Icon
function removeIcon() {
  $(".remove-img").on("click", function (e) {
    if (e.target.closest(".hidden-img")) {
      e.target.closest(".hidden-img").remove();
    } else if (e.target.closest(".upload-label")) {
      e.target.closest(".upload-label").remove();
    }
  });
}

removeIcon();

//================================ Telephone ============================//
var inputs = document.querySelectorAll(".telephone");

if (inputs) {

  for (var i = 0; i < inputs.length; i++) {
    let input = inputs[i];
    window.intlTelInput(input, {
      autoPlaceholder: "ادخل",
      initialCountry: "sa",
      separateDialCode: true,
    });
  }
}

//=========================== OTP =============================//
let codes = document.querySelectorAll(".code");

if ($(".modal")) {
  $(".modal").on("shown.bs.modal", function () {
    $(".code-container .code").first().focus();
    console.log($(".code-container .code").first());
    console.log('modal shown');
  });
} else {
  $(".code-container .code").first().focus();
}



codes.forEach((code, idx) => {
  code.addEventListener("keydown", (e) => {
    if (e.key >= 0 && e.key <= 9) {
      codes[idx].value = "";
      if ([idx + 1] < codes.length) {
        setTimeout(() => codes[idx + 1].focus(), 10);
      }
    } else if (e.key === "Backspace" && idx !== 0) {
      setTimeout(() => codes[idx - 1].focus(), 10);
    }
  });
});

//================================ Code Form ============================//
let arr = [];
$("form .main-btn").each(function () {
  $(this).on("click", function (e) {
    // e.preventDefault();
    $(this)
      .closest("form")
      .find(".code")
      .each(function () {
        arr.push($(this).val());
      });
    let code = arr.join("");
    $(this).closest("form").find(".all_code").val(code);
    arr = [];
  });
});

//================================ Select 2 ============================//
// Normal Select To
if ($(".select").length > 0) {
  $(".select").select2({
    dir: isRtl ? "rtl" : "ltr",
    minimumResultsForSearch: Infinity,
    placeholder: function () {
      $(this).data("placeholder");
    },
  });
}
