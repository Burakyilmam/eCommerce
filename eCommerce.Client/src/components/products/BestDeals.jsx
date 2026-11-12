import { useEffect, useRef, useState } from "react";
import { getProducts } from "../../services/productService.js";
import ProductCard from "./ProductCard.jsx";
import "./BestDeals.css";

function BestDeals(props) {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isDragging, setIsDragging] = useState(false);

    const productListRef = useRef(null);

    const dragRef = useRef({
        startX: 0,
        startScrollLeft: 0,
        isDragging: false,
    });

    useEffect(() => {
        const controller = new AbortController();

        async function loadProducts() {
            try {
                const data = await getProducts(controller.signal);

                if (!controller.signal.aborted) {
                    setProducts(
                        data.filter(product =>
                            product.isActive &&
                            product.isFeatured
                        )
                    );
                    setError(null);
                }
            } catch (err) {
                if (!controller.signal.aborted) {
                    setError(err.message);
                }
            } finally {
                if (!controller.signal.aborted) {
                    setLoading(false);
                }
            }
        }

        loadProducts();

        return () => controller.abort();
    }, []);

    function scrollProducts(direction) {
        const container = productListRef.current;

        if (!container) return;

        container.scrollBy({
            left: direction * 620,
            behavior: "smooth",
        });
    }

    function handlePointerDown(event) {
        if (event.target.closest("button")) return;
        if (event.button !== 0) return;

        const container = productListRef.current;

        if (!container) return;

        dragRef.current.startX = event.clientX;
        dragRef.current.startScrollLeft = container.scrollLeft;
        dragRef.current.isDragging = true;

        setIsDragging(true);
        event.currentTarget.setPointerCapture(event.pointerId);
    }

    function handlePointerMove(event) {
        if (!dragRef.current.isDragging) return;

        const container = productListRef.current;

        if (!container) return;

        const distance = event.clientX - dragRef.current.startX;

        container.scrollLeft =
            dragRef.current.startScrollLeft - distance;
    }

    function handlePointerUp() {
        dragRef.current.isDragging = false;
        setIsDragging(false);
    }

    function handlePointerCancel() {
        dragRef.current.isDragging = false;
        setIsDragging(false);
    }

    return (
        <section className="best-deals">
            <div className="best-deals-header">
                <h2>Öne Çıkan Ürünler</h2>

                <button
                    type="button"
                    className="view-all-button"
                >
                    Tümü ❯
                </button>
            </div>

            {loading && <p>Ürünler yükleniyor...</p>}

            {error && (
                <p role="alert">
                    Ürünler yüklenemedi: {error}
                </p>
            )}

            {!loading && !error && products.length === 0 && (
                <p>Henüz öne çıkan ürün bulunmuyor.</p>
            )}

            {!loading && !error && products.length > 0 && (
                <div className="best-deals-container">
                    <div
                        className={`product-list ${isDragging ? "is-dragging" : ""}`}
                        ref={productListRef}
                        onPointerDown={handlePointerDown}
                        onPointerMove={handlePointerMove}
                        onPointerUp={handlePointerUp}
                        onPointerCancel={handlePointerCancel}
                        onLostPointerCapture={handlePointerCancel}
                    >
                        {products.map(product => (
                            <ProductCard
                                key={product.id}
                                product={product}
                                onAddToCart={() => {
                                    props.onAddToCart?.(product);
                                }}
                            />
                        ))}
                    </div>

                    <button
                        type="button"
                        className="product-scroll-button product-scroll-left"
                        onClick={() => scrollProducts(-1)}
                        aria-label="Önceki ürünler"
                    >
                        ❮
                    </button>

                    <button
                        type="button"
                        className="product-scroll-button product-scroll-right"
                        onClick={() => scrollProducts(1)}
                        aria-label="Sonraki ürünler"
                    >
                        ❯
                    </button>
                </div>
            )}
        </section>
    );
}

export default BestDeals;