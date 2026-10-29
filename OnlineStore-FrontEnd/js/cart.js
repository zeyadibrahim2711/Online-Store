
const cartItems = document.querySelector("#cart-items");
const cartCount = document.querySelector("#cart-count");
const cartSubtotal = document.querySelector("#cart-subtotal");
const cartTotal = document.querySelector("#cart-total");
const cartLayout = document.querySelector("#cart-layout");
const emptyCart = document.querySelector("#empty-cart");

let cart = JSON.parse(localStorage.getItem("cart")) || [];

// Save cart to localStorage
function saveCart() {
    localStorage.setItem("cart", JSON.stringify(cart));
}

// Update cart counter
function updateCartCount() {
    let totalItems = 0;

    for (let i = 0; i < cart.length; i++) {
        totalItems += cart[i].quantity;
    }

    cartCount.textContent = totalItems;
}

// Display cart products
function displayCart() {
    cartItems.innerHTML = "";

    if (cart.length === 0) {
        cartLayout.hidden = true;
        emptyCart.hidden = false;

        cartSubtotal.textContent = "$0.00";
        cartTotal.textContent = "$0.00";

        updateCartCount();
        return;
    }

    cartLayout.hidden = false;
    emptyCart.hidden = true;

    let subtotal = 0;

    cart.forEach(function (product) {
        const itemTotal = product.price * product.quantity;
        subtotal += itemTotal;

        const article = document.createElement("article");
        article.className = "cart-item";

        article.innerHTML = `
            <div class="cart-item-image">${product.image}</div>

            <div class="cart-item-info">
                <h3>${product.name}</h3>
                <span class="cart-item-price">
                    $${product.price.toFixed(2)}
                </span>
            </div>

            <div class="cart-item-quantity">
                <label>Quantity</label>

                <div class="quantity-input">
                    <button type="button"
                            class="quantity-btn"
                            data-action="decrease"
                            data-id="${product.id}">−</button>

                    <input type="number"
                           value="${product.quantity}"
                           min="1"
                           max="10"
                           data-id="${product.id}"
                           class="quantity-value">

                    <button type="button"
                            class="quantity-btn"
                            data-action="increase"
                            data-id="${product.id}">+</button>
                </div>
            </div>

            <div class="cart-item-total">
                <span>Total</span>
                <strong>$${itemTotal.toFixed(2)}</strong>
            </div>

            <button type="button"
                    class="remove-btn"
                    data-id="${product.id}">
                Remove
            </button>
        `;

        cartItems.appendChild(article);
    });

    cartSubtotal.textContent = "$" + subtotal.toFixed(2);
    cartTotal.textContent = "$" + subtotal.toFixed(2);

    updateCartCount();
}

// Change product quantity
function changeQuantity(id, amount) {
    const product = cart.find(function (item) {
        return item.id === id;
    });

    if (!product) {
        return;
    }

    const newQuantity = product.quantity + amount;

    if (newQuantity < 1 || newQuantity > 10) {
        return;
    }

    product.quantity = newQuantity;

    saveCart();
    displayCart();
}


cartItems.addEventListener("click", function (event) {
    const button = event.target.closest("button");

    if (!button || !cartItems.contains(button)) {
        return;
    }

    const id = Number(button.dataset.id);

    if (button.classList.contains("remove-btn")) {
        cart = cart.filter(function (product) {
            return product.id !== id;
        });

        saveCart();
        displayCart();
        return;
    }

    if (button.dataset.action === "increase") {
        changeQuantity(id, 1);
    } else if (button.dataset.action === "decrease") {
        changeQuantity(id, -1);
    }
});


// Handle direct quantity input
cartItems.addEventListener("change", function (event) {
    if (!event.target.classList.contains("quantity-value")) {
        return;
    }

    const id = Number(event.target.dataset.id);
    const quantity = Number(event.target.value);

    const product = cart.find(function (item) {
        return item.id === id;
    });

    if (!product) {
        return;
    }

    if (!Number.isInteger(quantity) || quantity < 1 || quantity > 10) {
        displayCart();
        return;
    }

    product.quantity = quantity;

    saveCart();
    displayCart();
});

// Display cart when the page opens
displayCart();
