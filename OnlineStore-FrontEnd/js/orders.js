const orders = JSON.parse(localStorage.getItem("orders")) || [];
const cart = JSON.parse(localStorage.getItem("cart")) || [];

document.getElementById("cart-count").textContent =
    cart.reduce((total, item) => total + item.quantity, 0);
const ordersList = document.getElementById("orders-list");

if (orders.length === 0) {
    ordersList.innerHTML = `
        <div class="empty-orders">
            <h2>No Orders Yet</h2>
            <p>You haven't placed any orders yet.</p>
            <a href="products.html">Start Shopping</a>
        </div>
    `;
} else {
    orders.forEach(order => {
        let itemsHTML = "";

        order.items.forEach(item => {
            itemsHTML += `
        <p>${item.name} × ${item.quantity}</p>
    `;
        });
        // language=HTML
        ordersList.innerHTML += `
    <div class="order-card">
       <div class="order-header">
    <div>
        <h3 class="order-number">Order #${order.id}</h3>
        <p class="order-date">${order.date}</p>
    </div>

    <span class="order-status">${order.status}</span>
</div>

      <div class="order-details">
    <div class="order-items">
        ${itemsHTML}
    </div>

    <div class="order-total">
        <span>Order Total</span>
        <strong>$${order.total.toFixed(2)}</strong>
    </div>
</div>
<div class="order-actions">
    <a href="orders-details.html?id=${order.id}" class="order-details-btn">
        View Details
    </a>
</div>
    </div>
`;
    });
}