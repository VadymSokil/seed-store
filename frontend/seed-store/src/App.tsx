import { useEffect } from 'react';
import { Routes, Route } from 'react-router-dom';
import MainPage from './pages/main/MainPage';
import CatalogPage from './pages/catalog/CatalogPage';
import CategoryPage from './pages/category/CategoryPage';
import ProductPage from './pages/product/ProductPage';
import ComparePage from './pages/compare/ComparePage';
import SearchPage from './pages/search/SearchPage';
import FavoritesPage from './pages/favorites/FavoritesPage';
import CabinetPage from './pages/cabinet/CabinetPage';
import { getAccount } from './api/accountApi';
import { useAuthStore } from './store/useAuthStore';
import CheckoutPage from './pages/checkout/CheckoutPage';
import OrderSuccessPage from './pages/order-success/OrderSuccessPage';
import DiscountsPage from './pages/discounts/DiscountsPage';
import InfoPage from './pages/info/InfoPage';

function App() {
  const setAccount = useAuthStore(s => s.setAccount);

  useEffect(() => {
    getAccount()
      .then(setAccount)
      .catch(() => setAccount(null));
  }, []);

  return (
    <Routes>
      <Route path="/" element={<MainPage />} />
      <Route path="/catalog/*" element={<CatalogPage />} />
      <Route path="/category/*" element={<CategoryPage />} />
      <Route path="/product/:slug" element={<ProductPage />} />
      <Route path="/compare" element={<ComparePage />} />
      <Route path="/search" element={<SearchPage />} />
      <Route path="/favorites" element={<FavoritesPage />} />
      <Route path="/cabinet" element={<CabinetPage />} />
      <Route path="/checkout" element={<CheckoutPage />} />
      <Route path="/order-success" element={<OrderSuccessPage />} />
      <Route path="/discounts" element={<DiscountsPage />} />
      <Route path="/info" element={<InfoPage />}/>
    </Routes>
  );
}

export default App;