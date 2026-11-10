
import "./headerActions.css";

function HeaderActions(props) {
    // Ürün miktarını değiştir
    function changeQuantity(product, amount) {
        const newQuantity = product.quantity + amount;

        props.onUpdateCartQuantity(product.id, newQuantity);
    }

    // Ürünü tamamen sil
    function handleRemoveItem(product) {
        props.onRemoveFromCart(product.id);
    }

    // Tüm sepeti temizle
    function handleClearCart() {
        props.onClearCart();
    }

    // Sepet toplamı
    const cartTotal = props.cartItems.reduce(function (total, item) {
        return total + item.price * item.quantity;
    }, 0);

    return (
        <div className="header-actions">

            <button
                className="mobile-search-button"
                onClick={props.onSearchClick}
                aria-label="Aramayı aç veya kapat"
            >
                🔍
            </button>

            <div className="header-item">
                <span>♙</span>
                <span>Hesabım</span>
            </div>

            <div className="header-item">
                <span>♡</span>
                <span>Favorilerim</span>
            </div>

            <div className="header-item cart-item">

                <span>🛒</span>
                <span>Sepetim</span>

                {props.cartCount > 0 && (
                    <span className="cart-count">
                        {props.cartCount}
                    </span>
                )}

                <div className="cart-dropdown">
                    <div className="cart-dropdown-content">

                        {props.cartItems.length === 0 ? (

                            <p>Sepetiniz henüz boş.</p>

                        ) : (

                            <>

                                {/* SEPET BAŞLIĞI */}

                                <div className="cart-dropdown-header">
                                    <strong>Sepetim</strong>

                                    <span>
                                        {props.cartCount} adet
                                    </span>
                                </div>

                                {/* SEPETİ TEMİZLE */}

                                <button
                                    className="cart-clear-button"
                                    onClick={handleClearCart}
                                >
                                    Sepeti temizle
                                </button>

                                {/* ÜRÜN LİSTESİ */}

                                <div className="cart-products">

                                    {props.cartItems.map(function (item) {
                                        return (
                                            <div
                                                className="cart-product"
                                                key={item.id}
                                            >

                                                <img
                                                    className="cart-product-image"
                                                    src={item.image}
                                                    alt={item.name}
                                                />

                                                <div className="cart-product-info">

                                                    <strong className="cart-product-name">
                                                        {item.name}
                                                    </strong>

                                                    <span className="cart-product-unit-price">
                                                        {item.price.toLocaleString("tr-TR")} TL / adet
                                                    </span>

                                                    {/* MİKTAR KONTROLÜ */}

                                                    <div className="cart-quantity-control">

                                                        <button
                                                            className="quantity-button"
                                                            onClick={function () {
                                                                changeQuantity(item, -1);
                                                            }}
                                                            aria-label="Adedi azalt"
                                                        >
                                                            −
                                                        </button>

                                                        <span className="quantity-value">
                                                            {item.quantity}
                                                        </span>

                                                        <button
                                                            className="quantity-button"
                                                            onClick={function () {
                                                                changeQuantity(item, 1);
                                                            }}
                                                            aria-label="Adedi artır"
                                                        >
                                                            +
                                                        </button>

                                                    </div>

                                                </div>

                                                {/* ÜRÜN ARA TOPLAMI */}

                                                <strong className="cart-product-subtotal">
                                                    {(item.price * item.quantity).toLocaleString("tr-TR")} TL
                                                </strong>

                                                {/* ÜRÜNÜ TAMAMEN SİL */}

                                                <button
                                                    className="cart-product-delete"
                                                    onClick={function () {
                                                        handleRemoveItem(item);
                                                    }}
                                                    title="Ürünü tamamen sil"
                                                    aria-label={`${item.name} ürününü tamamen sil`}
                                                >
                                                    🗑️
                                                </button>

                                            </div>
                                        );
                                    })}

                                </div>

                                {/* SEPET TOPLAMI */}

                                <div className="cart-total">
                                    <strong>Toplam</strong>

                                    <strong>
                                        {cartTotal.toLocaleString("tr-TR")} TL
                                    </strong>
                                </div>

                                {/* SEPETE GİT */}

                                <button
                                    className="cart-go-button"
                                    onClick={props.onGoToCart}
                                >
                                    Sepete Git
                                    <span>→</span>
                                </button>

                            </>

                        )}

                    </div>
                </div>
            </div>
        </div>
    );
}

export default HeaderActions;
