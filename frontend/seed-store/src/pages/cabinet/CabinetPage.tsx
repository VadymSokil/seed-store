import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../../components/layout/Layout';
import { useAuthStore } from '../../store/useAuthStore';
import CabinetProfileSection from '../../components/cabinet/CabinetProfileSection';
import CabinetOrdersSection from '../../components/cabinet/CabinetOrdersSection';
import CabinetReviewsSection from '../../components/cabinet/CabinetReviewsSection';

type CabinetTab = 'profile' | 'orders' | 'reviews';

const tabs: { key: CabinetTab; label: string }[] = [
  { key: 'profile', label: 'Мій профіль' },
  { key: 'orders', label: 'Замовлення' },
  { key: 'reviews', label: 'Відгуки' },
];

export default function CabinetPage() {
  const navigate = useNavigate();
  const account = useAuthStore(s => s.account);
  const [activeTab, setActiveTab] = useState<CabinetTab>('profile');

  if (!account) return null;

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Кабінет</span>
        </nav>

        <h1 className="text-xl font-bold text-gray-800 mb-6">Ваш кабінет</h1>

        <div className="flex flex-col md:flex-row gap-0">
          <div className="flex md:flex-col overflow-x-auto scrollbar-hide md:overflow-visible shrink-0 md:w-48 border-b md:border-b-0 md:border-r border-gray-200 md:pr-6 mb-6 md:mb-0 gap-1">
            {tabs.map(tab => (
              <button
                key={tab.key}
                onClick={() => setActiveTab(tab.key)}
                className={`whitespace-nowrap text-left px-3 py-2 rounded-lg text-sm transition-colors
                  ${activeTab === tab.key
                    ? 'bg-green-50 text-green-700 font-medium'
                    : 'text-gray-500 hover:text-gray-800 hover:bg-gray-50'
                  }`}
              >
                {tab.label}
              </button>
            ))}
          </div>

          <div className="flex-1 md:pl-8">
            {activeTab === 'profile' && <CabinetProfileSection />}
            {activeTab === 'orders' && <CabinetOrdersSection />}
            {activeTab === 'reviews' && <CabinetReviewsSection />}
          </div>
        </div>
      </div>
    </Layout>
  );
}