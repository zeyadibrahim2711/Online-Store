
const productCards = document.querySelectorAll(".product-card");
const cartCount = document.querySelector("#cart-count");


let cart = JSON.parse(localStorage.getItem("cart")) || [];

// Update the cart counter
function updateCartCount() {
    let totalItems = 0;

    for (let i = 0; i < cart.length; i++) {
        totalItems += cart[i].quantity;
    }

    cartCount.textContent = totalItems;
}

// Add a product to the cart
function addToCart(product) {
    const existingProduct = cart.find(
        item => item.id === product.id
    );

    if (existingProduct) {
        existingProduct.quantity++;
    } else {
        cart.push({
            ...product,
            quantity: 1
        });
    }

    localStorage.setItem("cart", JSON.stringify(cart));

    updateCartCount();

    alert(product.name + " added to cart!");
}

// Handle Add to Cart buttons
productCards.forEach(card => {
    const button = card.querySelector(".add-to-cart");

    button.addEventListener("click", function () {
        const product = {
            id: Number(card.dataset.id),
            name: card.querySelector("h3").textContent,
            price: Number(
                card.querySelector(".product-price").textContent.replace("$", "")
            ),
            image: card.querySelector(".product-image").textContent
        };

        addToCart(product);
    });
});

// Display the saved cart count
updateCartCount();
function refreshCart() {
    cart = JSON.parse(localStorage.getItem("cart")) || [];
    updateCartCount();
}

window.addEventListener("pageshow", refreshCart);
