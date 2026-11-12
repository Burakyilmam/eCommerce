import { useState } from "react";
import "./headerActions.css";

function HeaderActions(props) {
    const [accountOpen, setAccountOpen] = useState(false);

    function openAuth(tab) {
        setAccountOpen(false);
        props.onOpenAuth(tab);
    }

    function changeQuantity(product, amount) {
        const newQuantity = product.quantity + amount;
        props.onUpdateCartQuantity(product.id, newQuantity);
    }

    function handleRemoveItem(product) {
        props.onRemoveFromCart(product.id);
    }

    function handleClearCart() {
        props.onClearCart();
    }

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

            <div
                className="account-menu"
                onMouseEnter={() => setAccountOpen(true)}
                onMouseLeave={() => setAccountOpen(false)}
            >
                <button
                    type="button"
                    className="header-item account-trigger"
                    aria-expanded={accountOpen}
                    aria-haspopup="true"
                    onClick={() => setAccountOpen(previous => !previous)}
                >
                    <span aria-hidden="true">♙</span>
                    <span>Hesabım</span>
                    <span className="account-chevron" aria-hidden="true">
                        ▾
                    </span>
                </button>

                {accountOpen && (
                    <div className="account-dropdown">
                        <div className="account-dropdown-content">
                            {props.user ? (
                                <>
                                    <div className="account-user-info">
                                        <strong>
                                            Merhaba,{" "}
                                            {props.user.firstName ||
                                                props.user.email}
                                        </strong>
                                        <small>{props.user.email}</small>
                                    </div>

                                    <div className="account-menu-links">
                                        <button type="button" disabled>
                                            ♙ Profilim
                                            <small>Yakında</small>
                                        </button>

                                        <button type="button" disabled>
                                            📦 Siparişlerim
                                            <small>Yakında</small>
                                        </button>

                                        <button type="button" disabled>
                                            ♡ Favorilerim
                                            <small>Yakında</small>
                                        </button>

                                        <button
                                            type="button"
                                            className="account-logout"
                                            onClick={() => {
                                                setAccountOpen(false);
                                                props.onLogout();
                                            }}
                                        >
                                            ↪ Çıkış Yap
                                        </button>
                                    </div>
                                </>
                            ) : (
                                <>
                                    <p className="account-welcome">
                                        Hoş geldiniz!
                                    </p>

                                    <p className="account-description">
                                        Hesabınıza giriş yapın veya hemen üye olun.
                                    </p>

                                    <div className="account-auth-buttons">
                                        <button
                                            type="button"
                                            className="account-login-button"
                                            onClick={() => openAuth("login")}
                                        >
                                            Giriş Yap
                                        </button>

                                        <button
                                            type="button"
                                            className="account-register-button"
                                            onClick={() => openAuth("register")}
                                        >
                                            Üye Ol
                                        </button>
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                )}
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
                                <div className="cart-dropdown-header">
                                    <strong>Sepetim</strong>
                                    <span>
                                        {props.cartCount} adet
                                    </span>
                                </div>

                                <button
                                    className="cart-clear-button"
                                    onClick={handleClearCart}
                                >
                                    Sepeti temizle
                                </button>

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

                                                <strong className="cart-product-subtotal">
                                                    {(item.price * item.quantity).toLocaleString("tr-TR")} TL
                                                </strong>

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

                                <div className="cart-total">
                                    <strong>Toplam</strong>
                                    <strong>
                                        {cartTotal.toLocaleString("tr-TR")} TL
                                    </strong>
                                </div>

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