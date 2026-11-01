
// Products Data
const products = [
    {
        id: 1,
        name: "Headphones",
        image: "🎧",
        description: "Comfortable headphones for everyday use. Enjoy your favorite music with ease.",
        price: 49.99,
        quantity: 10
    },
    {
        id: 2,
        name: "Smart Watch",
        image: "⌚",
        description: "A smart watch to help you stay connected and track your daily activities.",
        price: 79.99,
        quantity: 15
    },
    {
        id: 3,
        name: "Backpack",
        image: "🎒",
        description: "A practical backpack suitable for everyday use and carrying your essentials.",
        price: 34.99,
        quantity: 20
    },
    {
        id: 4,
        name: "Wireless Mouse",
        image: "🖱️",
        description: "A wireless mouse designed for comfortable and easy everyday use.",
        price: 19.99,
        quantity: 25
    }
];

// Get Product ID from URL
const urlParams = new URLSearchParams(window.location.search);
const productId = Number(urlParams.get("id"));

// Find Product
const product = products.find(p => p.id === productId);

// Check if Product Exists
if (!product) {
    alert("Product not found!");
    window.location.href = "products.html";
} else {

    // Read Reviews Link
    const readReviewsLink = document.querySelector("#read-reviews");
    readReviewsLink.href = `reviews.html?id=${product.id}`;

    // Display Product Details
    document.querySelector(".product-details-image").textContent =
        product.image;

    document.querySelector(".product-details-info h1").textContent =
        product.name;

    document.querySelector(".details-description").textContent =
        product.description;

    document.querySelector(".details-price").textContent =
        `$${product.price}`;

    document.querySelector(".details-quantity span").textContent =
        product.quantity;

    // Quantity Control
    const quantityInput = document.getElementById("quantity");
    const quantityButtons = document.querySelectorAll(".quantity-btn");

    // Set Maximum Quantity
    quantityInput.max = product.quantity;

    // Handle Quantity Buttons
    quantityButtons.forEach((button, index) => {

        button.addEventListener("click", function () {

            let quantity = Number(quantityInput.value);

            if (index === 0 && quantity > 1) {
                quantity--;
            }

            if (index === 1 && quantity < product.quantity) {
                quantity++;
            }

            quantityInput.value = quantity;
        });
    });

    // Handle Manual Quantity Input
    quantityInput.addEventListener("change", function () {

        let quantity = Number(quantityInput.value);

        if (!Number.isInteger(quantity) || quantity < 1) {
            quantity = 1;
        }

        if (quantity > product.quantity) {
            quantity = product.quantity;
        }

        quantityInput.value = quantity;
    });

    // Get Cart from localStorage
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Save Cart
    function saveCart() {
        localStorage.setItem("cart", JSON.stringify(cart));
    }

    // Update Cart Count
    function updateCartCount() {

        const cartCount = document.getElementById("cart-count");

        const totalQuantity = cart.reduce(
            (total, item) => total + item.quantity,
            0
        );

        cartCount.textContent = totalQuantity;
    }

    // Add to Cart
    const addToCartButton = document.querySelector(".add-to-cart");

    addToCartButton.addEventListener("click", function () {

        const selectedQuantity = Number(quantityInput.value);

        const existingProduct = cart.find(
            item => item.id === product.id
        );

        const currentQuantity = existingProduct
            ? existingProduct.quantity
            : 0;

        if (currentQuantity + selectedQuantity > product.quantity) {
            alert("Not enough stock available!");
            return;
        }

        if (existingProduct) {
            existingProduct.quantity += selectedQuantity;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: product.price,
                image: product.image,
                quantity: selectedQuantity
            });
        }

        saveCart();
        updateCartCount();

        alert("Product added to cart!");
    });

    // Initial Cart Count
    updateCartCount();
    function refreshCart() {
        cart = JSON.parse(localStorage.getItem("cart")) || [];
        updateCartCount();
    }
    window.addEventListener("pageshow", refreshCart);
}