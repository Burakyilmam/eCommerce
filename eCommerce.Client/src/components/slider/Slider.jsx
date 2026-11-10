
import { useState, useEffect, useRef } from "react";
import { getSliders } from "../../services/sliderService";
import "./slider.css";

const SLIDE_DURATION = 8000;
const DRAG_THRESHOLD = 50;

function Slider() {
    const [slides, setSlides] = useState([]);
    const [activeSlide, setActiveSlide] = useState(0);
    const [isDragging, setIsDragging] = useState(false);
    const [isPaused, setIsPaused] = useState(false);
    const [error, setError] = useState(null);

    const dragStartX = useRef(0);
    const dragStartY = useRef(0);
    const dragPointerId = useRef(null);

    const slideCount = slides.length;

    // Slider verilerini API'den al
    useEffect(() => {
        const controller = new AbortController();

        async function loadSliders() {
            try {
                const data = await getSliders(controller.signal);

                if (!controller.signal.aborted) {
                    setSlides(data);
                    setActiveSlide(0);
                    setError(null);
                }
            } catch (err) {
                if (!controller.signal.aborted) {
                    console.error("Slider hatası:", err);
                    setError(err.message);
                }
            }
        }

        loadSliders();

        return () => controller.abort();
    }, []);

    // Sonraki slayt
    function nextSlide() {
        if (slideCount <= 1) return;

        setActiveSlide(current =>
            (current + 1) % slideCount
        );
    }

    // Önceki slayt
    function previousSlide() {
        if (slideCount <= 1) return;

        setActiveSlide(current =>
            (current - 1 + slideCount) % slideCount
        );
    }

    // Sürükleme başlangıcı
    function handlePointerDown(event) {
        if (slideCount <= 1) return;

        // Buton tıklamalarını sürükleme olarak algılama
        if (event.target.closest("button")) return;

        // Mouse için yalnızca sol tuş
        if (event.pointerType === "mouse" && event.button !== 0) {
            return;
        }

        dragStartX.current = event.clientX;
        dragStartY.current = event.clientY;
        dragPointerId.current = event.pointerId;

        setIsDragging(true);

        event.currentTarget.setPointerCapture(event.pointerId);
    }

    // Sürükleme bitişi
    function handlePointerUp(event) {
        if (
            dragPointerId.current === null ||
            dragPointerId.current !== event.pointerId
        ) {
            return;
        }

        const distanceX =
            event.clientX - dragStartX.current;

        const distanceY =
            event.clientY - dragStartY.current;

        // Yatay sürükleme dikey hareketten büyükse geçiş yap
        if (
            Math.abs(distanceX) >= DRAG_THRESHOLD &&
            Math.abs(distanceX) > Math.abs(distanceY)
        ) {
            if (distanceX > 0) {
                previousSlide();
            } else {
                nextSlide();
            }
        }

        dragPointerId.current = null;
        setIsDragging(false);

        if (event.currentTarget.hasPointerCapture(event.pointerId)) {
            event.currentTarget.releasePointerCapture(event.pointerId);
        }
    }

    // Sürükleme iptali
    function handlePointerCancel(event) {
        if (dragPointerId.current !== event.pointerId) {
            return;
        }

        dragPointerId.current = null;
        setIsDragging(false);
    }

    // Görsele tıklayarak duraklat / devam ettir
    function togglePause() {
        setIsPaused(previous => !previous);
    }

    // Progress bar tamamlandığında slaytı değiştir
    function handleProgressEnd(event) {
        if (event.target !== event.currentTarget) {
            return;
        }

        if (
            !isPaused &&
            !isDragging &&
            slideCount > 1
        ) {
            nextSlide();
        }
    }

    if (error) {
        return null;
    }

    if (slideCount === 0) {
        return null;
    }

    const currentSlide =
        slides[activeSlide] ?? slides[0];

    return (
        <section
            className={`slider ${isDragging ? "is-dragging" : ""
                }`}
            onPointerDown={handlePointerDown}
            onPointerUp={handlePointerUp}
            onPointerCancel={handlePointerCancel}
        >
            {/* Görsel */}
            <div
                className="slider-image"
                onClick={togglePause}
                title={isPaused ? "Oynat" : "Duraklat"}
            >
                <img
                    src={currentSlide.image}
                    alt={
                        currentSlide.imageAlt ??
                        currentSlide.title
                    }
                    draggable="false"
                />
            </div>

            {/* Kampanya bilgileri */}
            <div className="slider-content">
                <h2>{currentSlide.title}</h2>

                <h3>{currentSlide.highlight}</h3>

                <div className="slider-discount">
                    {currentSlide.discount}
                </div>

                <p>{currentSlide.description}</p>
            </div>

            {/* Önceki slayt */}
            <button
                type="button"
                className="slider-arrow slider-prev"
                onClick={previousSlide}
                disabled={slideCount <= 1}
                aria-label="Önceki kampanya"
            >
                &#10094;
            </button>

            {/* Sonraki slayt */}
            <button
                type="button"
                className="slider-arrow slider-next"
                onClick={nextSlide}
                disabled={slideCount <= 1}
                aria-label="Sonraki kampanya"
            >
                &#10095;
            </button>

            {/* Slayt sayacı */}
            <div className="slider-counter">
                {activeSlide + 1} / {slideCount}
            </div>

            {/* Progress bar */}
            <div className="slider-progress">
                <div
                    key={activeSlide}
                    className={`slider-progress-bar ${isDragging || isPaused
                            ? "paused"
                            : ""
                        }`}
                    style={{
                        animationDuration: `${SLIDE_DURATION}ms`,
                        animationPlayState:
                            isDragging || isPaused
                                ? "paused"
                                : "running"
                    }}
                    onAnimationEnd={handleProgressEnd}
                />
            </div>
        </section>
    );
}

export default Slider;
