function getCartItems() {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    let cartItemsContainer = document.getElementById("cartItemsContainer");
    let itemCount = document.getElementById("itemCount");
    let itemsTotal = document.getElementById("itemsTotal");
    let totalPrice = document.getElementById("totalPrice");
    let finalTotalPrice = document.getElementById("finalTotalPrice");
    let shippingOption = document.getElementById("shippingOption");

    let totalAmount = 0;
    let itemCountValue = cart.length;

    cartItemsContainer.innerHTML = '';

    cart.forEach((item, index) => {
        let itemHTML = `
                <div class="row mb-4 d-flex justify-content-between align-items-center">
                    <div class="col-md-2 col-lg-2 col-xl-2">
                        <img src="${item.Image || '/images/Books/default.jpg'}"
                        class="img-fluid rounded-3" alt="${item.Title}"
                         onerror="this.src='/images/Books/default.jpg'" />
                    </div>
                    <div class="col-md-3 col-lg-3 col-xl-3">
                        <h6 class="text-muted">${item.Categories}</h6>
                        <h6 class="mb-0">${item.Title}</h6>
                        <h6 class="text-muted">By: </h6>
                        <h6 class="mb-0">${item.Authors}</h6>
                    </div>
                    <div class="col-md-3 col-lg-3 col-xl-2 d-flex">
                        <button class="btn btn-link px-2" onclick="decreaseQuantity(${index})">
                            <i class="fas fa-minus"></i>
                        </button>
                        <input type="number" min="1" value="${item.Quantity}"
                               class="form-control form-control-sm"
                               onchange="updateQuantity(${index}, this.value)">
                        <button class="btn btn-link px-2" onclick="increaseQuantity(${index})">
                            <i class="fas fa-plus"></i>
                        </button>
                    </div>
                    <div class="col-md-3 col-lg-2 col-xl-2 offset-lg-1">
                        <h6 class="mb-0">€ ${(item.Price * item.Quantity).toFixed(2)}</h6>
                    </div>
                 <div class="col-md-1 col-lg-1 col-xl-1 text-end">
                <button class="btn btn-link text-danger" onclick="removeItem(${index})">
                    <i class="fas fa-trash-alt fa-lg"></i>
                 </button>
                 </div>
                </div>
                <hr class="my-4">
            `;
        cartItemsContainer.innerHTML += itemHTML;
        totalAmount += item.Price * item.Quantity;
    });

    // تحديث القيم
    const shippingCost = parseFloat(shippingOption.value);
    const finalTotal = totalAmount + shippingCost;

    itemCount.textContent = `${itemCountValue} ${itemCountValue === 1 ? 'item' : 'items'}`;
    itemsTotal.textContent = itemCountValue;
    totalPrice.textContent = `€ ${totalAmount.toFixed(2)}`;
    finalTotalPrice.textContent = `€ ${finalTotal.toFixed(2)}`;
}

function updateQuantity(index, newQuantity) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    newQuantity = Math.max(1, parseInt(newQuantity));
    cart[index].Quantity = newQuantity;
    localStorage.setItem("cart", JSON.stringify(cart));
    getCartItems();
}

function increaseQuantity(index) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    cart[index].Quantity += 1;
    localStorage.setItem("cart", JSON.stringify(cart));
    getCartItems();
}

function decreaseQuantity(index) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    if (cart[index].Quantity > 1) {
        cart[index].Quantity -= 1;
        localStorage.setItem("cart", JSON.stringify(cart));
        getCartItems();
    }
}

function removeItem(index) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    cart.splice(index, 1);
    localStorage.setItem("cart", JSON.stringify(cart));
    getCartItems();
}
document.addEventListener("DOMContentLoaded", function () {
    let buttons = document.querySelectorAll(".add-to-cart");
    buttons.forEach(btn => btn.addEventListener("click", function () {
        try {
            // معالجة authors و categories مع تحقق أفضل قبل التحويل
            let authors = this.dataset.authors ? JSON.parse(this.dataset.authors) : [];
            let categories = this.dataset.categories ? JSON.parse(this.dataset.categories) : [];

            let book = {
                ID: this.dataset.id,
                Title: this.dataset.title,
                Price: parseFloat(this.dataset.price),
                Image: this.dataset.image || '/images/Books/default.jpg',
                Authors: authors,
                Publisher: this.dataset.publisher || "Unknown",
                Categories: categories,
                Quantity: 1
            };

            // تحقق من أن السعر ليس NaN قبل إضافة الكتاب للسلة
            if (isNaN(book.Price)) {
                alert("Price is not valid for this book.");
                return;
            }

            // استدعاء دالة إضافة الكتاب إلى السلة
            AddToCart(book);
        } catch (error) {
            console.error("Error adding book to cart:", error);
            alert("There was an error while adding the book to the cart.");
        }
    }));
});
function AddToCart(book) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // تحقق مما إذا كان الكتاب موجودًا بالفعل في السلة
    let existingItem = cart.find(item => item.ID == book.ID);
    if (existingItem) {
        existingItem.Quantity += 1; // زيادة الكمية إذا كان موجودًا
    } else {
        cart.push(book); // إضافة العنصر إذا لم يكن موجودًا
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    alert("Added to cart: " + book.Title); // ✅ رسالة تأكيد للمستخدم

}
function updateCartCount() {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    document.getElementById("cartCount").textContent = cart.length;
}

// تحديث عند تغيير خيار الشحن
document.getElementById('shippingOption').addEventListener('change', getCartItems);

// التحميل الأولي
document.addEventListener("DOMContentLoaded", getCartItems);

function SaveCartToServerDebounced()
{
    const cart = JSON.parse(localStorage.getItem("cart")) || [];
    await fetch("/Cart/Sync", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(cart)
    });
}