import { useEffect, useMemo, useState } from 'react';
import { getCategories } from '../../services/categoryService.js';
import CategoryTree from './CategoryTree.jsx';
import './categories.css';

function buildCategoryTree(categories, parentId = null) {
    return categories
        .filter(category => category.parentCategoryId === parentId)
        .sort((a, b) => a.displayOrder - b.displayOrder)
        .map(category => ({
            ...category,
            children: buildCategoryTree(categories, category.id)
        }));
}

function Categories(props) {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [selectedCategory, setSelectedCategory] = useState(0);
    const [sidebarWidth, setSidebarWidth] = useState(165);

    const [activeDesktopCategory, setActiveDesktopCategory] = useState(null);
    const [desktopMenuLeft, setDesktopMenuLeft] = useState(0);

    useEffect(() => {
        const controller = new AbortController();

        async function loadCategories() {
            try {
                const data = await getCategories(controller.signal);

                if (!Array.isArray(data)) {
                    throw new Error('API beklenen kategori listesini döndürmedi.');
                }

                if (!controller.signal.aborted) {
                    setCategories(data);
                    setSelectedCategory(0);
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

        loadCategories();

        return () => controller.abort();
    }, []);

    const categoryTree = useMemo(() => {
        const activeCategories = categories.filter(
            category => category.isActive
        );

        return buildCategoryTree(activeCategories);
    }, [categories]);

    const selectedData = categoryTree[selectedCategory];

    const activeDesktopData = categoryTree.find(
        category => category.id === activeDesktopCategory
    );

    function handleResizeStart(event) {
        event.currentTarget.setPointerCapture(event.pointerId);
    }

    function handleResizeMove(event) {
        if (event.buttons !== 1) {
            return;
        }

        const container = event.currentTarget.parentElement;
        const rect = container.getBoundingClientRect();

        let newWidth = event.clientX - rect.left;

        if (newWidth < 115) {
            newWidth = 115;
        }

        if (newWidth > 320) {
            newWidth = 320;
        }

        setSidebarWidth(newWidth);
    }

    function handleDesktopCategoryHover(event, categoryId) {
        const button = event.currentTarget;
        const nav = button.closest('.desktop-categories');

        if (!nav) {
            return;
        }

        const buttonRect = button.getBoundingClientRect();
        const navRect = nav.getBoundingClientRect();

        const menuWidth = Math.min(380, navRect.width - 20);

        let newLeft = buttonRect.left - navRect.left;

        const maxLeft = Math.max(0, navRect.width - menuWidth);

        if (newLeft > maxLeft) {
            newLeft = maxLeft;
        }

        if (newLeft < 0) {
            newLeft = 0;
        }

        setDesktopMenuLeft(newLeft);
        setActiveDesktopCategory(categoryId);
    }

    function handleDesktopCategoryClick(categoryId) {
        setActiveDesktopCategory(previous =>
            previous === categoryId ? null : categoryId
        );
    }

    if (loading) {
        return (
            <div className="categories">
                <div className="category-content">
                    Kategoriler yükleniyor...
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="categories">
                <div className="category-content">
                    Kategoriler yüklenemedi: {error}
                </div>
            </div>
        );
    }

    if (categoryTree.length === 0) {
        return (
            <div className="categories">
                <div className="category-content">
                    Gösterilecek ana kategori bulunmuyor.
                </div>
            </div>
        );
    }

    return (
        <div className="categories">
            <nav
                className="desktop-categories"
                onMouseLeave={() => setActiveDesktopCategory(null)}
            >
                <div className="desktop-category-list">
                    {categoryTree.map(category => (
                        <button
                            key={category.id}
                            type="button"
                            className={
                                activeDesktopCategory === category.id
                                    ? "desktop-category-item active"
                                    : "desktop-category-item"
                            }
                            onMouseEnter={event =>
                                handleDesktopCategoryHover(
                                    event,
                                    category.id
                                )
                            }
                            onClick={() =>
                                handleDesktopCategoryClick(category.id)
                            }
                        >
                            {category.name}
                        </button>
                    ))}
                </div>

                {activeDesktopData &&
                    activeDesktopData.children.length > 0 && (
                        <div
                            className="desktop-mega-menu"
                            style={{
                                left: desktopMenuLeft + "px"
                            }}
                            onMouseEnter={() =>
                                setActiveDesktopCategory(
                                    activeDesktopData.id
                                )
                            }
                        >
                            <div className="desktop-mega-menu-header">
                                <h3>{activeDesktopData.name}</h3>

                                <button
                                    type="button"
                                    aria-label="Menüyü kapat"
                                    onClick={() =>
                                        setActiveDesktopCategory(null)
                                    }
                                >
                                    ✕
                                </button>
                            </div>

                            <div className="desktop-mega-menu-content">
                                {activeDesktopData.children.map(category => (
                                    <CategoryTree
                                        key={category.id}
                                        category={category}
                                        defaultOpen={false}
                                    />
                                ))}
                            </div>
                        </div>
                    )}
            </nav>

            <div
                className={
                    props.isOpen
                        ? "mobile-categories open"
                        : "mobile-categories"
                }
                style={{
                    gridTemplateColumns:
                        sidebarWidth + "px 6px minmax(0, 1fr)"
                }}
            >
                <div className="category-sidebar">
                    {categoryTree.map((category, index) => (
                        <button
                            key={category.id}
                            type="button"
                            className={
                                selectedCategory === index
                                    ? "category-sidebar-item active"
                                    : "category-sidebar-item"
                            }
                            onClick={() =>
                                setSelectedCategory(index)
                            }
                        >
                            {category.icon && (
                                <span>{category.icon}</span>
                            )}
                            <span>{category.name}</span>
                        </button>
                    ))}
                </div>

                <div
                    className="category-resizer"
                    onPointerDown={handleResizeStart}
                    onPointerMove={handleResizeMove}
                    role="separator"
                    aria-orientation="vertical"
                    aria-label="Kategori listesinin genişliğini ayarla"
                />

                <div className="category-content">
                    {(selectedData?.children ?? []).map(category => (
                        <CategoryTree
                            key={category.id}
                            category={category}
                            defaultOpen={true}
                        />
                    ))}
                </div>
            </div>
        </div>
    );
}

export default Categories;