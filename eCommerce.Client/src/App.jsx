
import { useState } from "react";

import Header from "./components/header/header.jsx";
import Categories from "./components/categories/categories.jsx";
import Slider from "./components/slider/Slider.jsx";
import BestDeals from "./components/products/BestDeals.jsx";

import Swal from "sweetalert2";

import "./App.css";

function App() {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [cartItems, setCartItems] = useState([]);

    function toggleMenu() {
        setIsMenuOpen(function (previousState) {
            return !previousState;
        });
    }

    // Sepete ürün ekle
    function addToCart(product) {
        setCartItems(function (currentItems) {
            const existingItem = currentItems.find(function (item) {
                return item.id === product.id;
            });

            if (existingItem) {
                return currentItems.map(function (item) {
                    if (item.id === product.id) {
                        return {
                            ...item,
                            quantity: item.quantity + 1,
                        };
                    }

                    return item;
                });
            }

            return [
                ...currentItems,
                {
                    ...product,
                    quantity: 1,
                },
            ];
        });

        Swal.fire({
            toast: true,
            position: "top-end",
            icon: "success",
            title: "Ürün sepete eklendi!",
            text: product.name,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
        });
    }

    // Ürün miktarını güncelle
    function updateCartQuantity(productId, newQuantity) {
        setCartItems(function (currentItems) {
            return currentItems
                .map(function (item) {
                    if (item.id === productId) {
                        return {
                            ...item,
                            quantity: newQuantity,
                        };
                    }

                    return item;
                })
                .filter(function (item) {
                    return item.quantity > 0;
                });
        });
    }

    // Ürünü tamamen sepetten kaldır
    function removeFromCart(productId) {
        setCartItems(function (currentItems) {
            return currentItems.filter(function (item) {
                return item.id !== productId;
            });
        });
    }

    // Sepetin tamamını temizle
    function clearCart() {
        setCartItems([]);
    }

    // Sepetteki toplam ürün adedi
    const cartCount = cartItems.reduce(function (total, item) {
        return total + item.quantity;
    }, 0);

    return (
        <div className="app">

            <Header
                onMenuClick={toggleMenu}
                cartCount={cartCount}
                cartItems={cartItems}
                onUpdateCartQuantity={updateCartQuantity}
                onRemoveFromCart={removeFromCart}
                onClearCart={clearCart}
            />

            <Categories isOpen={isMenuOpen} />

            <Slider />

            <main>
                <BestDeals onAddToCart={addToCart} />
            </main>

        </div>
    );
}

export default App;
