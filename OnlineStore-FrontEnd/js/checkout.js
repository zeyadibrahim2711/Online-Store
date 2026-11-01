const cart = JSON.parse(localStorage.getItem("cart")) || [];
const cartCount = document.getElementById("cart-count");

cartCount.textContent = cart.reduce((total, item) => total + item.quantity, 0);
const summaryItems = document.getElementById("summary-items");

const totalItems = cart.reduce((total, item) => total + item.quantity, 0);

summaryItems.textContent = `${totalItems} items in your cart`;
const summaryProducts = document.getElementById("summary-products");

summaryProducts.innerHTML = "";

cart.forEach(item => {
    summaryProducts.innerHTML += `
        <div class="summary-row">
            <span>${item.name} × ${item.quantity}</span>
            <span>$${(item.price * item.quantity).toFixed(2)}</span>
        </div>
    `;
});
const subtotal = cart.reduce((total, item) => {
    return total + (item.price * item.quantity);
}, 0);

document.getElementById("subtotal").textContent = `$${subtotal.toFixed(2)}`;
document.getElementById("total").textContent = `$${subtotal.toFixed(2)}`;
if (cart.length === 0) {
    alert("Your cart is empty. Please add products first.");
    window.location.href = "cart.html";
}
const checkoutForm = document.getElementById("checkout-form");
checkoutForm.addEventListener("submit", function (event) {

    event.preventDefault();
    if (cart.length === 0) {
        alert("Your cart is empty.");
        return;
    }

    const customer = {
        name: document.getElementById("name").value.trim(),
        email: document.getElementById("email").value.trim(),
        address: document.getElementById("address").value.trim(),
        phone: document.getElementById("phone").value.trim(),
        payment: document.getElementById("payment").value
    };
    if (
        customer.name === "" ||
        customer.email === "" ||
        customer.address === "" ||
        customer.phone === ""
    ) {
        alert("Please fill in all required fields.");
        return;
    }
    if (!/^01[0125][0-9]{8}$/.test(customer.phone)) {
        alert("Please enter a valid Egyptian phone number.");
        return;
    }
    const order = {
        id: Date.now(),
        customer: customer,
        items: cart,
        subtotal: subtotal,
        total: subtotal,
        date: new Date().toLocaleDateString(),
        status: "Pending"
    };
    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    orders.push(order);

    localStorage.setItem("orders", JSON.stringify(orders));
    localStorage.removeItem("cart");
    alert("Your order has been placed successfully!");

    window.location.href = "orders.html";
});