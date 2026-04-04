import { Routes, Route } from 'react-router-dom';
import MainPage from './pages/main/MainPage';
import CatalogPage from './pages/catalog/CatalogPage';
import CategoryPage from './pages/category/CategoryPage';
import ProductPage from './pages/product/ProductPage';
import ComparePage from './pages/compare/ComparePage';
import SearchPage from './pages/search/SearchPage';
import FavoritesPage from './pages/favorites/FavoritesPage';

function App() {
  return (
    <Routes>
      <Route path="/" element={<MainPage />} />
      <Route path="/catalog/*" element={<CatalogPage />} />
      <Route path="/category/*" element={<CategoryPage />} />
      <Route path="/product/:slug" element={<ProductPage />} />
      <Route path="/compare" element={<ComparePage />} />
      <Route path="/search" element={<SearchPage />} />
      <Route path="/favorites" element={<FavoritesPage />} />
    </Routes>
  );
}

export default App;