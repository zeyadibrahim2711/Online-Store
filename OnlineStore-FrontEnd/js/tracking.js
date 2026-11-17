
// Get order ID from URL
const params = new URLSearchParams(window.location.search);

const orderId = params.get("id");

// Get orders from localStorage
const orders = JSON.parse(localStorage.getItem("orders")) || [];

// Find selected order
const order = orders.find(item => item.id == orderId);

// Get cart from localStorage
const cart = JSON.parse(localStorage.getItem("cart")) || [];

// Update cart count
document.getElementById("cart-count").textContent =
    cart.reduce((total, item) => total + item.quantity, 0);

// Get tracking content container
const trackingContent = document.getElementById("tracking-content");

// Check if order exists
if (!order) {

    trackingContent.innerHTML = `
        <div class="empty-orders">
            <h2>Order Not Found</h2>
            <p>We couldn't find this order.</p>
            <a href="orders.html">Back to My Orders</a>
        </div>
    `;

} else {

    // Delivery progress steps
    const steps = [
        "Pending",
        "Processing",
        "Shipped",
        "Out for Delivery",
        "Delivered"
    ];

    // Get current order status
    const currentStep = steps.indexOf(order.status);

    // Create delivery progress HTML
    let progressHTML = "";

    steps.forEach((step, index) => {

        const completed = index <= currentStep;

        progressHTML += `
            <div class="tracking-step ${completed ? "completed" : ""}">

                <div class="tracking-step-icon">
                    ${completed ? "✓" : index + 1}
                </div>

                <span class="tracking-step-title">
                    ${step}
                </span>

            </div>
        `;

    });

    // Display tracking information
    trackingContent.innerHTML = `

        <section class="tracking-card">

            <h2>Order Information</h2>

            <div class="tracking-info">

                <div class="tracking-info-item">
                    <span class="tracking-label">Order Number</span>
                    <span class="tracking-value">#${order.id}</span>
                </div>

                <div class="tracking-info-item">
                    <span class="tracking-label">Order Date</span>
                    <span class="tracking-value">${order.date}</span>
                </div>

                <div class="tracking-info-item">
                    <span class="tracking-label">Order Status</span>
                    <span class="tracking-status">${order.status}</span>
                </div>

            </div>

        </section>

        <section class="tracking-card">

            <h2>Delivery Progress</h2>

            <div class="tracking-progress">
                ${progressHTML}
            </div>

        </section>
        <section class="tracking-card">

    <h2>Shipping Information</h2>

    <div class="tracking-shipping">

        <div class="tracking-shipping-row">
            <span class="tracking-shipping-label">Customer Name</span>
            <span class="tracking-shipping-value">
                ${order.customer.name}
            </span>
        </div>

        <div class="tracking-shipping-row">
            <span class="tracking-shipping-label">Shipping Address</span>
            <span class="tracking-shipping-value">
                ${order.customer.address}
            </span>
        </div>

    </div>

</section>

<a href="orders.html" class="tracking-back-btn">
    ← Back to My Orders
</a>

    `;

}
