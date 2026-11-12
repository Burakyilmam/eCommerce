import { useState } from 'react';
import './categoryTree.css';

function CategoryTree(props) {
    const [isOpen, setIsOpen] = useState(
        props.defaultOpen !== undefined
            ? props.defaultOpen
            : false
    );

    const hasChildren =
        props.category.children &&
        props.category.children.length > 0;

    function handleClick() {
        if (hasChildren) {
            setIsOpen(function (previousState) {
                return !previousState;
            });
        } else {
            console.log("Seçilen kategori:", props.category.name);
        }
    }

    return (
        <div className="category-tree-node">
            <div className="category-tree-row">
                <button
                    type="button"
                    className="category-tree-button"
                    onClick={handleClick}
                >
                    {props.category.name}
                </button>

                {hasChildren && (
                    <button
                        type="button"
                        className="category-tree-toggle"
                        onClick={handleClick}
                        aria-label="Alt kategorileri aç veya kapat"
                        aria-expanded={isOpen}
                    >
                        <span className={isOpen ? "arrow open" : "arrow"}>
                            ⌄
                        </span>
                    </button>
                )}
            </div>

            {hasChildren && (
                <div
                    className={
                        isOpen
                            ? "category-tree-children open"
                            : "category-tree-children"
                    }
                >
                    {props.category.children.map(function (child) {
                        return (
                            <CategoryTree
                                key={child.id}
                                category={child}
                                defaultOpen={props.defaultOpen}
                            />
                        );
                    })}
                </div>
            )}
        </div>
    );
}

export default CategoryTree;