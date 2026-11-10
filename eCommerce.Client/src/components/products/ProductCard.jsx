
import { useState } from "react";
import "./ProductCard.css";

function ProductCard({ product, onAddToCart }) {
    const [isFavorite, setIsFavorite] = useState(false);

    const price = product.price;
    const oldPrice = product.oldPrice;

    const hasPrice =
        typeof price === "number" &&
        Number.isFinite(price);

    const hasOldPrice =
        typeof oldPrice === "number" &&
        Number.isFinite(oldPrice) &&
        hasPrice &&
        oldPrice > price;

    const discount = hasOldPrice
        ? Math.round((1 - price / oldPrice) * 100)
        : null;

    function toggleFavorite() {
        setIsFavorite(previousState => !previousState);
    }

    return (
        <article className="product-card">
            <div className="product-image-container">
                {product.image ? (
                    <img
                        src={product.image}
                        alt={product.name}
                        className="product-image"
                        loading="lazy"
                    />
                ) : (
                    <div className="product-image">
                        Görsel yok
                    </div>
                )}

                {product.badge && (
                    <span className="product-badge">
                        {product.badge}
                    </span>
                )}

                <button
                    type="button"
                    className={`favorite-button ${isFavorite ? "favorite-active" : ""
                        }`}
                    onClick={toggleFavorite}
                    aria-label={
                        isFavorite
                            ? "Favorilerden çıkar"
                            : "Favorilere ekle"
                    }
                >
                    {isFavorite ? "♥" : "♡"}
                </button>
            </div>

            <div className="product-details">
                <h3 className="product-name">
                    {product.name}
                </h3>

                {product.rating != null && (
                    <div className="product-rating">
                        <span>★</span>
                        <strong>{product.rating}</strong>

                        {product.reviewCount != null && (
                            <small>
                                ({product.reviewCount})
                            </small>
                        )}
                    </div>
                )}

                {product.installment && (
                    <div className="product-installment">
                        Peşin fiyatına {product.installment}
                    </div>
                )}

                <div className="product-price-area">
                    <div className="product-prices">
                        {hasOldPrice && (
                            <div className="product-old-price">
                                <del>
                                    {oldPrice.toLocaleString("tr-TR")} TL
                                </del>

                                <span>%{discount}</span>
                            </div>
                        )}

                        <div className="product-current-price">
                            {hasPrice
                                ? `${price.toLocaleString("tr-TR")} TL`
                                : "Fiyat bilgisi yok"}
                        </div>
                    </div>

                    <button
                        type="button"
                        className="add-to-cart-button"
                        onClick={onAddToCart}
                        disabled={!hasPrice}
                        aria-label="Sepete ekle"
                        title={
                            hasPrice
                                ? "Sepete Ekle"
                                : "Fiyat bilgisi bulunmuyor"
                        }
                    >
                        🛒
                    </button>
                </div>
            </div>
        </article>
    );
}

export default ProductCard;
