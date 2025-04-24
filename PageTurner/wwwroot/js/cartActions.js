document.addEventListener("DOMContentLoaded", function () {
    
    window.addToCart = function (bookID) {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenInput) {
            alert("CSRF token not found!");
            return;
        }

        const formData = new FormData();
        formData.append("bookID", bookID);
        formData.append("__RequestVerificationToken", tokenInput.value);

        fetch('/Cart/AddToCart', {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert("Added to cart successfully!");
                    updateCartCount(0,1); 
                } else {
                    alert("Failed to add to cart: " + data.message);
                }
            });
    };

    document.querySelectorAll(".quantity-input").forEach(input => {
        input.addEventListener('change', async function () {
            const item = this.closest('.cart-item');
            const bookID = item.getAttribute('data-bookID');
            const oldQuantity = parseInt(item.querySelector('.quantity-input').value);
            const newQuantity = parseInt(this.value);
            const price = parseFloat(item.getAttribute('data-price'));
            const totalPriceSpan = item.querySelector('span');

            const newTotal = (newQuantity * price).toFixed(2);
            totalPriceSpan.textContent = `x €${newTotal}`;

            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            const response = await fetch('/Cart/UpdateQuantity', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ bookID: parseInt(bookID), quantity: parseInt(newQuantity) })
            });

            if (response.ok) {
                if (newQuantity > oldQuantity) {
                    updateCartCount(0, newQuantity - oldQuantity); 
                } else if (newQuantity < oldQuantity) {
                    updateCartCount(newQuantity - oldQuantity, 0); 
                } 
            } else {
                alert("Can't update Quantity!");
            }
        });
    });

    document.querySelectorAll('.remove-btn').forEach(function (button) {
        button.addEventListener('click', function (event) {
            const itemElement = event.target.closest('.cart-item');
            const cartItemID = itemElement.getAttribute('data-cartItemID');
            const bookID = itemElement.getAttribute('data-bookID');
            const quantity = parseInt(itemElement.querySelector('.quantity-input').value);
            const url = '/Cart/RemoveItem';

            const formData = new URLSearchParams();
            formData.append("cartItemID", cartItemID);
            formData.append("bookID", bookID);
            formData.append("__RequestVerificationToken", document.querySelector('input[name="__RequestVerificationToken"]').value);

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        itemElement.remove();
                        alert('Item removed successfully!');
                        updateCartCount(quantity); 
                    } else {
                        alert('Failed to remove item: ' + data.message);
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred.');
                });
        });
    });

    function updateCartCount(decrementBy = 0, incrementBy = 0) {
        const badge = document.getElementById('cartCount');
        if (badge) {
            let currentCount = parseInt(badge.textContent);
            badge.textContent = Math.max(0, currentCount - decrementBy + incrementBy);
        }
    }

    updateCartCount();
});
