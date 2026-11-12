
import { useState } from "react";
import Header from "./components/header/header.jsx";
import Categories from "./components/categories/categories.jsx";
import Slider from "./components/slider/Slider.jsx";
import BestDeals from "./components/products/BestDeals.jsx";
import AuthPage from "./components/auth/AuthPage.jsx";
import ResetPasswordPage from "./components/auth/ResetPasswordPage.jsx";
import ConfirmEmailPage from "./components/auth/ConfirmEmailPage.jsx";
import Swal from "sweetalert2";
import "./App.css";

const AUTH_STORAGE_KEY = "hepsibende_auth";

function getJwtExpiration(accessToken) {
    try {
        const parts = accessToken.split(".");

        if (parts.length !== 3) {
            return null;
        }

        const base64 = parts[1]
            .replace(/-/g, "+")
            .replace(/_/g, "/");

        const payload = JSON.parse(
            atob(base64.padEnd(
                Math.ceil(base64.length / 4) * 4,
                "="
            ))
        );

        return typeof payload.exp === "number"
            ? payload.exp * 1000
            : null;
    } catch {
        return null;
    }
}

function isSessionValid(session) {
    if (!session || !session.accessToken) {
        return false;
    }

    const expiration = getJwtExpiration(
        session.accessToken
    );

    if (!expiration) {
        return false;
    }

    return expiration > Date.now();
}

function clearStoredSession() {
    try {
        localStorage.removeItem(AUTH_STORAGE_KEY);
        sessionStorage.removeItem(AUTH_STORAGE_KEY);
    } catch {

    }
}

function loadStoredSession() {
    try {
        const localSession = localStorage.getItem(
            AUTH_STORAGE_KEY
        );

        if (localSession) {
            const parsed = JSON.parse(localSession);

            if (isSessionValid(parsed)) {
                return parsed;
            }

            localStorage.removeItem(AUTH_STORAGE_KEY);
        }

        const tabSession = sessionStorage.getItem(
            AUTH_STORAGE_KEY
        );

        if (tabSession) {
            const parsed = JSON.parse(tabSession);

            if (isSessionValid(parsed)) {
                return parsed;
            }

            sessionStorage.removeItem(AUTH_STORAGE_KEY);
        }
    } catch {
        clearStoredSession();
    }

    return null;
}

function saveSession(session, rememberMe) {
    try {
        clearStoredSession();

        const storage = rememberMe
            ? localStorage
            : sessionStorage;

        storage.setItem(
            AUTH_STORAGE_KEY,
            JSON.stringify(session)
        );
    } catch {

    }
}

function App() {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [cartItems, setCartItems] = useState([]);
    const [isAuthOpen, setIsAuthOpen] = useState(false);

    const [user, setUser] = useState(loadStoredSession);

    const [authInitialTab, setAuthInitialTab] =
        useState("login");

    function openAuth(tab) {
        setAuthInitialTab(tab);
        setIsAuthOpen(true);
    }

    function toggleMenu() {
        setIsMenuOpen(previousState => !previousState);
    }

    function handleLogin(authResponse, rememberMe = false) {
        const session = {
            userId: authResponse.userId,
            email: authResponse.email,
            firstName: authResponse.firstName,
            lastName: authResponse.lastName,
            roles: authResponse.roles,
            accessToken: authResponse.accessToken,
            expiresAt: authResponse.expiresAt
        };

        setUser(session);

        saveSession(session, rememberMe);

        setIsAuthOpen(false);

        Swal.fire({
            toast: true,
            position: "top-end",
            icon: "success",
            title: "Giriş başarılı!",
            showConfirmButton: false,
            timer: 2000
        });
    }

    function handleLogout() {
        clearStoredSession();
        setUser(null);
        setIsAuthOpen(false);
    }

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
                            quantity: item.quantity + 1
                        };
                    }

                    return item;
                });
            }

            return [
                ...currentItems,
                {
                    ...product,
                    quantity: 1
                }
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
            timerProgressBar: true
        });
    }

    function updateCartQuantity(productId, newQuantity) {
        setCartItems(function (currentItems) {
            return currentItems
                .map(function (item) {
                    if (item.id === productId) {
                        return {
                            ...item,
                            quantity: newQuantity
                        };
                    }

                    return item;
                })
                .filter(function (item) {
                    return item.quantity > 0;
                });
        });
    }

    function removeFromCart(productId) {
        setCartItems(function (currentItems) {
            return currentItems.filter(function (item) {
                return item.id !== productId;
            });
        });
    }

    function clearCart() {
        setCartItems([]);
    }

    const cartCount = cartItems.reduce(function (total, item) {
        return total + item.quantity;
    }, 0);

    if (window.location.pathname === "/reset-password") {
        return <ResetPasswordPage />;
    }

    if (window.location.pathname === "/confirm-email") {
        return <ConfirmEmailPage />;
    }

    return (
        <div className="app">
            <Header
                onMenuClick={toggleMenu}
                cartCount={cartCount}
                cartItems={cartItems}
                onUpdateCartQuantity={updateCartQuantity}
                onRemoveFromCart={removeFromCart}
                onClearCart={clearCart}
                user={user}
                onOpenAuth={openAuth}
                onLogout={handleLogout}
            />

            <Categories isOpen={isMenuOpen} />

            <Slider />

            <main>
                <BestDeals onAddToCart={addToCart} />
            </main>

            {isAuthOpen && (
                <AuthPage
                    initialTab={authInitialTab}
                    onLogin={handleLogin}
                    onClose={() => setIsAuthOpen(false)}
                />
            )}
        </div>
    );
}

export default App;
