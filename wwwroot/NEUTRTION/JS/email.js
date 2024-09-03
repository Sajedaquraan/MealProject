//Selectors
let messageNotSent = document.getElementById("message-not-sent-container");
let messageSent = document.getElementById("message-sent-container");
let forms = document.getElementsByClassName('needs-validation');

//Initialise emailJS
(function () {
    emailjs.init("user_Oay0ciCVLZ3t84nUMqX94");
})();

//Bootstrap form validation
(function() {
    'use strict';
    window.addEventListener('load', function() {
      // Loop over them and prevent submission
      let validation = Array.prototype.filter.call(forms, function(form) {
        form.addEventListener('submit', function(event) {
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            messageNotSent.style.display = "block";
          } else { 
            //send email with emailJS
            event.preventDefault();
            emailjs.sendForm('gmail', 'milestone2', '#contact-form')
                .then(function (response) {
                    console.log('SUCCESS!', response.status, response.text);
                    if (messageNotSent.style.display = "block") {
                      messageNotSent.style.display = "none";
                      messageSent.style.display = "block";
                    }
                }, function (error) {
                    console.log('FAILED...', error);
                });
          }
          form.classList.add('was-validated');
        }, false);
      });
    }, false);
  })();