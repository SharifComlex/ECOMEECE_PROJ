var stripe = Stripe('pk_test_51LA8LSA2T7xDNF0lcox0OMdCr6TPlfcVrOEAnsG9U60MZvW5dJPGw2UzFaX97YifPEKX9mEMFga0rp5OXgm7R6Qs00GpwxB9LP');

// Create an instance of Elements.
var elements = stripe.elements();
// Custom styling can be passed to options when creating an Element.
var style = {
    base: {
        // Add your base input styles here. For example:
        fontSize: '16px',
        color: "#32325d",
    }
};

// Create an instance of the card Element.
var card = elements.create('card', {
    hidePostalCode: true,
    style: {
        base: {
            iconColor: '#F99A52',
            color: '#32315E',
            lineHeight: '48px',
            fontWeight: 400,
            fontFamily: '"Open Sans", "Helvetica Neue", "Helvetica", sans-serif',
            fontSize: '15px',

            '::placeholder': {
                color: '#CFD7DF',
            }
        },
    }
});

// Add an instance of the card Element into the `card-element` <div>.
card.mount('#card-element');

var form = document.getElementById('payment-form');
form.addEventListener('submit', function (event) {
    event.preventDefault();

    stripe.createToken(card).then(function (result) {
        if (result.error) {
            // Inform the customer that there was an error.
            var errorElement = document.getElementById('card-errors');
            errorElement.textContent = result.error.message;
        } else {
            // Send the token to your server.
            stripeTokenHandler(result.token);
        }
    });
});

function stripeTokenHandler(token) {
    // Insert the token ID into the form so it gets submitted to the server
    var form = document.getElementById('payment-form');
    var hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'stripeToken');
    hiddenInput.setAttribute('value', token.id);
    form.appendChild(hiddenInput);

    // Submit the form
    form.submit();
}