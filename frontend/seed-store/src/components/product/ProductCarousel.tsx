import useEmblaCarousel from 'embla-carousel-react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import ProductCard from './ProductCard';

interface Product {
  id: number;
  slug: string;
  name: string;
  imageUrl: string;
  price: number;
  oldPrice?: number;
  rating: number;
  reviewsCount: number;
  inStock: boolean;
}

interface ProductCarouselProps {
  title: string;
  products: Product[];
}

const ProductCarousel = ({ title, products }: ProductCarouselProps) => {
  const [emblaRef, emblaApi] = useEmblaCarousel({ 
    loop: false, 
    align: 'start', 
    containScroll: 'keepSnaps',
    dragFree: false
  });

  const scrollPrev = () => emblaApi?.scrollPrev();
  const scrollNext = () => emblaApi?.scrollNext();

  return (
    <div className="py-6">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-bold text-gray-800">{title}</h2>
        <div className="flex gap-2">
          <button onClick={scrollPrev} className="p-1.5 rounded-full border border-gray-300 text-gray-400 hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors">
            <ChevronLeft size={20} strokeWidth={2.5} />
          </button>
          <button onClick={scrollNext} className="p-1.5 rounded-full border border-gray-300 text-gray-400 hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors">
            <ChevronRight size={20} strokeWidth={2.5} />
          </button>
        </div>
      </div>
      <div className="overflow-hidden" ref={emblaRef}>
        <div className="flex gap-4">
          {products.map(product => (
            <div key={product.id} className="flex-none w-64 py-2">
              <ProductCard {...product} />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default ProductCarousel;