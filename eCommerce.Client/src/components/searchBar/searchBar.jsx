import './searchBar.css';
function SearchBar() {
    return (
        <div className="search-container">
            <span className="search-icon">⌕</span>

            <input
                type="text"
                placeholder="Ürün, kategori veya marka ara"
            />
        </div>
    );
}

export default SearchBar;
