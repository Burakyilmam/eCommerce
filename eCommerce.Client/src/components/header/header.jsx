import { useState } from "react";
import SearchBar from "../searchBar/searchBar.jsx";
import HeaderActions from "../headerActions/headerActions.jsx";
import "./header.css";

function Header(props) {
    const [isSearchOpen, setIsSearchOpen] = useState(false);

    function toggleSearch() {
        setIsSearchOpen(function (previousState) {
            return !previousState;
        });
    }

    return (
        <header className={isSearchOpen ? "header search-open" : "header"}>
            <div className="header-brand">
                <button
                    className="menu-button"
                    onClick={props.onMenuClick}
                    aria-label="Kategorileri aç veya kapat"
                >
                    ☰
                </button>

                <img
                    src="/src/assets/images/logo.png"
                    alt="HepsiBende"
                    className="logo"
                />
            </div>

            <SearchBar />

            <HeaderActions
                onSearchClick={toggleSearch}
                cartCount={props.cartCount}
                cartItems={props.cartItems}
                onUpdateCartQuantity={props.onUpdateCartQuantity}
                onRemoveFromCart={props.onRemoveFromCart}
                onClearCart={props.onClearCart}
                onGoToCart={props.onGoToCart}
                user={props.user}
                onOpenAuth={props.onOpenAuth}
                onLogout={props.onLogout}
            />
        </header>
    );
}

export default Header;