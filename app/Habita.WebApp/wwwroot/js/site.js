(function (window) {
  "use strict";

                                                                 

  function formatDate(iso, options) {
    if (!iso) return "";
    var defaultOpts = {
      day: "2-digit",
      month: "short",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    };
    try {
      return new Date(iso).toLocaleDateString(
        "es-DO",
        Object.assign({}, defaultOpts, options || {}),
      );
    } catch (e) {
      return iso;
    }
  }

  function formatTimeAgo(iso) {
    if (!iso) return "";
    var diff = Math.floor((Date.now() - new Date(iso).getTime()) / 1000);
    if (diff < 60) return "hace unos segundos";
    if (diff < 3600) return "hace " + Math.floor(diff / 60) + " min";
    if (diff < 86400) return "hace " + Math.floor(diff / 3600) + " h";
    if (diff < 604800) return "hace " + Math.floor(diff / 86400) + " d";
    return formatDate(iso, { hour: undefined, minute: undefined });
  }

  function debounce(fn, ms) {
    var t;
    return function () {
      var args = arguments;
      var ctx = this;
      clearTimeout(t);
      t = setTimeout(function () {
        fn.apply(ctx, args);
      }, ms);
    };
  }

  function getRequestVerificationToken() {
    var input = document.querySelector(
      'input[name="__RequestVerificationToken"]',
    );
    return input ? input.value : "";
  }

                                                                 
                
                                                                 

  var ToastContainer = null;
  function ensureToastContainer() {
    if (!ToastContainer) {
      ToastContainer = document.createElement("div");
      ToastContainer.id = "toast-container";
      document.body.appendChild(ToastContainer);
    }
    return ToastContainer;
  }

  const Toast = (() => {
    function show(type, message, opts) {
      opts = opts || {};
      var colors = {
        success: "toast-success",
        error: "toast-error",
        warning: "toast-warning",
        info: "toast-info",
      };
      var icons = {
        success: "check-circle",
        error: "x-circle",
        warning: "alert-triangle",
        info: "info",
      };
      var el = document.createElement("div");
      el.setAttribute("role", "alert");
      el.innerHTML =
        '<i data-lucide="' +
        icons[type] +
        '" class="w-5 h-5 shrink-0"></i><span class="text-sm font-medium flex-1"></span>';
      el.querySelector("span").textContent = message;
      el.className = colors[type];
      el.style.animation =
        "toastEnter 0.3s cubic-bezier(0.16,1,0.3,1) forwards";
      ensureToastContainer().appendChild(el);
      if (window.lucide) window.lucide.createIcons();
      var dur = opts.duration || 4000;
      setTimeout(function () {
        el.style.opacity = "0";
        el.style.transform = "translateX(100%)";
        el.style.transition = "all 0.3s";
        setTimeout(function () {
          el.remove();
        }, 300);
      }, dur);
    }
    return {
      success: function (msg, opts) {
        show("success", msg, opts);
      },
      error: function (msg, opts) {
        show("error", msg, opts);
      },
      warning: function (msg, opts) {
        show("warning", msg, opts);
      },
      info: function (msg, opts) {
        show("info", msg, opts);
      },
    };
  })();

  function confirmAction(title, text, confirmText) {
    return new Promise(function (resolve) {
      if (window.Swal) {
        Swal.fire({
          title: title || "¿Está seguro?",
          text: text || "Esta acción no se puede deshacer.",
          icon: "warning",
          showCancelButton: true,
          confirmButtonText: confirmText || "Sí, confirmar",
          cancelButtonText: "Cancelar",
          confirmButtonColor: "#2563eb",
          cancelButtonColor: "#6b7280",
          reverseButtons: true,
        }).then(function (r) {
          resolve(r.isConfirmed);
        });
      } else {
        resolve(confirm(title || "¿Está seguro?"));
      }
    });
  }

  function initPasswordToggles() {
    document.querySelectorAll("[data-toggle-password]").forEach(function (btn) {
      if (btn.dataset.bound) return;
      btn.dataset.bound = "1";
      btn.setAttribute("aria-pressed", "false");
      btn.setAttribute("aria-label", "Mostrar contraseña");
      btn.textContent = "Mostrar";
      btn.addEventListener("click", function () {
        var input = document.getElementById(
          btn.getAttribute("data-toggle-password"),
        );
        if (!input) return;
        var isPwd = input.type === "password";
        input.type = isPwd ? "text" : "password";
        btn.setAttribute("aria-pressed", isPwd ? "true" : "false");
        btn.setAttribute(
          "aria-label",
          isPwd ? "Ocultar contraseña" : "Mostrar contraseña",
        );
        btn.textContent = isPwd ? "Ocultar" : "Mostrar";
      });
    });
  }

  function initPasswordStrength() {
    var input = document.getElementById("password-input");
    if (!input || input.dataset.bound) return;
    input.dataset.bound = "1";
    input.addEventListener("input", function () {
      var val = this.value;
      var checks = [
        val.length >= 8,
        /[A-Z]/.test(val),
        /[a-z]/.test(val),
        /[0-9]/.test(val),
        /[^A-Za-z0-9]/.test(val),
      ];
      var score = checks.filter(Boolean).length;
      var bar = document.getElementById("strength-bar");
      var text = document.getElementById("strength-text");
      if (bar && text) {
        var colors = [
          "bg-red-500",
          "bg-red-500",
          "bg-yellow-500",
          "bg-blue-500",
          "bg-emerald-500",
        ];
        var labels = ["Muy débil", "Débil", "Media", "Fuerte", "Muy fuerte"];
        var idx = score === 0 ? 0 : score - 1;
        bar.className =
          "h-full transition-all duration-300 rounded-full " + colors[idx];
        bar.style.width = score * 20 + "%";
        text.textContent = "Fortaleza: " + labels[idx];
      }
    });
  }

  function initImagePreview(inputSelector, previewSelector) {
    var fileInput = document.querySelector(
      inputSelector || 'input[type="file"]',
    );
    var preview = document.querySelector(previewSelector || "#image-preview");
    if (fileInput && preview && !fileInput.dataset.bound) {
      fileInput.dataset.bound = "1";
      fileInput.addEventListener("change", function () {
        if (this.files && this.files[0]) {
          preview.src = URL.createObjectURL(this.files[0]);
          preview.classList.remove("hidden");
        } else {
          preview.classList.add("hidden");
        }
      });
    }
  }

  function initTabs() {
    document.querySelectorAll("[data-tabs]").forEach(function (container) {
      var buttons = container.querySelectorAll(".tab-btn");
      var contents = container.querySelectorAll(".tab-content");
      buttons.forEach(function (btn) {
        if (btn.dataset.bound) return;
        btn.dataset.bound = "1";
        btn.addEventListener("click", function () {
          var tab = btn.dataset.tab;
          buttons.forEach(function (b) {
            b.classList.remove(
              "surface-elevated",
              "shadow-sm",
              "text-brand-600",
              "dark:text-brand-500",
            );
            b.classList.add("text-gray-600", "dark:text-gray-400");
          });
          btn.classList.add(
            "surface-elevated",
            "shadow-sm",
            "text-brand-600",
            "dark:text-brand-500",
          );
          btn.classList.remove("text-gray-600", "dark:text-gray-400");
          contents.forEach(function (c) {
            c.classList.add("hidden");
          });
          var target = container.querySelector("#tab-" + tab);
          if (target) target.classList.remove("hidden");
        });
      });
    });
  }

  function initUserDropdown() {
    var btn = document.getElementById("user-menu-btn");
    var dd = document.getElementById("user-dropdown");
    if (!btn || !dd || btn.dataset.bound) return;
    btn.dataset.bound = "1";
    btn.addEventListener("click", function (e) {
      e.stopPropagation();
      dd.classList.toggle("hidden");
      btn.setAttribute("aria-expanded", !dd.classList.contains("hidden"));
    });
    document.addEventListener("click", function (e) {
      if (!btn.contains(e.target)) {
        dd.classList.add("hidden");
        btn.setAttribute("aria-expanded", "false");
      }
    });
  }

  function closeAllDropdowns(except) {
    var pairs = [{ btnId: "user-menu-btn", ddId: "user-dropdown" }];
    pairs.forEach(function (pair) {
      if (pair.btnId === except) return;
      var btn = document.getElementById(pair.btnId);
      var dd = document.getElementById(pair.ddId);
      if (dd && !dd.classList.contains("hidden")) {
        dd.classList.add("hidden");
        if (btn) btn.setAttribute("aria-expanded", "false");
      }
    });
  }

                            
  document.addEventListener("submit", function (e) {
    var form = e.target.closest("form[data-confirm]");
    if (!form || form.dataset.confirmBound) return;
    form.dataset.confirmBound = "1";
    e.preventDefault();
    var title = form.dataset.confirm || "¿Está seguro?";
    var text = form.dataset.confirmText || "Esta acción no se puede deshacer.";
    confirmAction(title, text).then(function (ok) {
      delete form.dataset.confirmBound;
      if (ok) form.submit();
    });
  });

  function submitDynamicPost(url, values) {
    var form = document.createElement("form");
    form.method = "POST";
    form.action = url;
    var token = document.createElement("input");
    token.type = "hidden";
    token.name = "__RequestVerificationToken";
    token.value = getRequestVerificationToken();
    form.appendChild(token);
    Object.keys(values || {}).forEach(function (key) {
      var input = document.createElement("input");
      input.type = "hidden";
      input.name = key;
      input.value = values[key];
      form.appendChild(input);
    });
    document.body.appendChild(form);
    form.submit();
  }

  function initPhoneMasks() {
    document.querySelectorAll("[data-phone-mask]").forEach(function (input) {
      if (input.dataset.bound) return;
      input.dataset.bound = "1";
      input.addEventListener("input", function () {
        var value = input.value.replace(/\D/g, "").slice(0, 10);
        if (value.length > 6)
          value =
            value.slice(0, 3) + "-" + value.slice(3, 6) + "-" + value.slice(6);
        else if (value.length > 3)
          value = value.slice(0, 3) + "-" + value.slice(3);
        input.value = value;
      });
    });
  }

                                                                 
                
                                                                 

  window.Habita = {

    formatDate: formatDate,
    formatTimeAgo: formatTimeAgo,
    debounce: debounce,
    getRequestVerificationToken: getRequestVerificationToken,
    Toast: Toast,
    confirmAction: confirmAction,
    initPasswordToggles: initPasswordToggles,
    initPasswordStrength: initPasswordStrength,
    initImagePreview: initImagePreview,
    initTabs: initTabs,
    initUserDropdown: initUserDropdown,
    initPhoneMasks: initPhoneMasks,
    submitDynamicPost: submitDynamicPost,
  };

                                                                 
                                  
                                                                 

  document.addEventListener("DOMContentLoaded", function () {

    initPasswordToggles();
    initPasswordStrength();
    initImagePreview();
    initTabs();
    initPhoneMasks();
    initUserDropdown();
    if (window.lucide) lucide.createIcons();
  });
})(window);
