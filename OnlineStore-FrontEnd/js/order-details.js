// Get order ID from URL
const params = new URLSearchParams(window.location.search);

const orderId = params.get("id");

// Get orders from localStorage
const orders = JSON.parse(localStorage.getItem("orders")) || [];

// Get cart from localStorage
const cart = JSON.parse(localStorage.getItem("cart")) || [];

// Update cart count
document.getElementById("cart-count").textContent =
    cart.reduce((total, item) => total + item.quantity, 0);

// Find selected order
const order = orders.find(item => item.id == orderId);

// Get order details container
const orderDetails = document.getElementById("order-details");
console.log("URL ID:", orderId);
console.log("Found Order:", order);
// Check if order exists
if (!order) {

    orderDetails.innerHTML = `
<div class="empty-orders">
    <h2>Order Not Found</h2>
<p>We couldn't find this order.</p>
<a href="orders.html">Back to My Orders</a>
</div>
`;

} else {

    // Create products HTML
    let productsHTML = "";

    order.items.forEach(item => {

        productsHTML += `
<div class="order-product">

    <div class="order-product-info">

<div class="product-image">${item.image}</div>
    <h3>${item.name}</h3>

<p>Quantity: ${item.quantity}</p>

</div>

<span class="order-product-price">
                    $${(item.price * item.quantity).toFixed(2)}
                </span>

</div>
`;

    });

    // Display order details
    orderDetails.innerHTML = `

<section class="order-detail-card">

    <h2>Order Information</h2>

<div class="detail-row">
    <span class="detail-label">Order Number</span>
    <span class="detail-value">#${order.id}</span>
</div>

<div class="detail-row">
    <span class="detail-label">Order Date</span>
    <span class="detail-value">${order.date}</span>
</div>

<div class="detail-row">
    <span class="detail-label">Status</span>
    <span class="detail-status">${order.status}</span>
</div>

</section>


<section class="order-detail-card">

    <h2>Shipping Information</h2>

    <div class="detail-row">
        <span class="detail-label">Full Name</span>
        <span class="detail-value">${order.customer.name}</span>
    </div>

    <div class="detail-row">
        <span class="detail-label">Address</span>
        <span class="detail-value">${order.customer.address}</span>
    </div>

    <div class="detail-row">
        <span class="detail-label">Payment</span>
        <span class="detail-value">${order.customer.payment}</span>
    </div>

</section>


<section class="order-detail-card order-detail-products">

    <h2>Ordered Products</h2>

    ${productsHTML}

    <div class="order-detail-total">

        <span>Total Amount</span>

        <strong>
            $${order.total.toFixed(2)}
        </strong>

    </div>

</section>

    `;

}
