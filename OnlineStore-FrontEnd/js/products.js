// Get cart from localStorage
let cart = JSON.parse(localStorage.getItem("cart")) || [];


// Save cart to localStorage
function saveCart() {
    localStorage.setItem("cart", JSON.stringify(cart));
}


// Update cart count
function updateCartCount() {
    const cartCount = document.getElementById("cart-count");

    const totalQuantity = cart.reduce(
        (total, item) => total + item.quantity,
        0
    );

    cartCount.textContent = totalQuantity;
}


// Add product to cart
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

    saveCart();
    updateCartCount();

    alert("Product added to cart!");
}


// Add event listeners to buttons
const addToCartButtons = document.querySelectorAll(".btn-add-cart");

addToCartButtons.forEach(button => {
    button.addEventListener("click", function () {

        const productCard = button.closest(".product-card");

        const product = {
            id: Number(productCard.dataset.id),
            name: productCard.querySelector("h3").textContent,
            price: Number(
                productCard.querySelector(".product-price")
                    .textContent.replace("$", "")
            ),
            image: productCard.querySelector(".product-image").textContent
        };

        addToCart(product);
    });
});




// Display products
const productGrid = document.querySelector(".product-grid");

const allProducts = Array.from(
    productGrid.querySelectorAll(".product-card")
);


function displayProducts() {

    const searchValue = document
        .getElementById("search-input")
        .value.toLowerCase();

    const selectedCategory = document
        .getElementById("category-filter")
        .value;

    const sortValue = document
        .getElementById("sort-filter")
        .value;


    // Filter products
    let filteredProducts = allProducts.filter(product => {

        const productName = product
            .querySelector("h3")
            .textContent.toLowerCase();

        const productCategory = product.dataset.category;

        const matchesSearch = productName.includes(searchValue);

        const matchesCategory =
            selectedCategory === "" ||
            selectedCategory === productCategory;

        return matchesSearch && matchesCategory;
    });


    // Sort products
    if (sortValue === "price-asc") {

        filteredProducts.sort((a, b) => {

            const priceA = Number(
                a.querySelector(".product-price").textContent.replace("$", "")
            );

            const priceB = Number(
                b.querySelector(".product-price").textContent.replace("$", "")
            );

            return priceA - priceB;
        });

    } else if (sortValue === "price-desc") {

        filteredProducts.sort((a, b) => {

            const priceA = Number(
                a.querySelector(".product-price").textContent.replace("$", "")
            );

            const priceB = Number(
                b.querySelector(".product-price").textContent.replace("$", "")
            );

            return priceB - priceA;
        });
    }


    // Display filtered products
    allProducts.forEach(product => {
        product.style.display = "none";
    });

    filteredProducts.forEach(product => {
        product.style.display = "";
        productGrid.appendChild(product);
    });
}


// Search event
document.getElementById("search-input")
    .addEventListener("input", displayProducts);


// Category event
document.getElementById("category-filter")
    .addEventListener("change", displayProducts);


// Sort event
document.getElementById("sort-filter")
    .addEventListener("change", displayProducts);


// Initial cart count
updateCartCount();
function refreshCart() {
    cart = JSON.parse(localStorage.getItem("cart")) || [];
    updateCartCount();
}
window.addEventListener("pageshow", refreshCart);