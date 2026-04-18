import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Layout from '../../components/layout/Layout';
import { getAboutPage, getContacts, getDeliveryVariants, getPaymentVariants } from '../../api/storeInfoApi';
import type { StoreInfoVariant } from '../../api/storeInfoApi';

type InfoTab = 'about' | 'delivery' | 'contacts';

const tabs: { key: InfoTab; label: string }[] = [
  { key: 'about', label: 'Про нас' },
  { key: 'delivery', label: 'Оплата і доставка' },
  { key: 'contacts', label: 'Контакти' },
];

export default function InfoPage() {
  const navigate = useNavigate();
  const { hash } = useLocation();

  const initialTab = (): InfoTab => {
    if (hash === '#delivery') return 'delivery';
    if (hash === '#contacts') return 'contacts';
    return 'about';
  };

  const [activeTab, setActiveTab] = useState<InfoTab>(initialTab);
  const [aboutContent, setAboutContent] = useState('');
  const [contactsContent, setContactsContent] = useState('');
  const [deliveryVariants, setDeliveryVariants] = useState<StoreInfoVariant[]>([]);
  const [paymentVariants, setPaymentVariants] = useState<StoreInfoVariant[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    Promise.all([getAboutPage(), getContacts(), getDeliveryVariants(), getPaymentVariants()])
      .then(([about, contacts, delivery, payment]) => {
        setAboutContent(about);
        setContactsContent(contacts);
        setDeliveryVariants(delivery);
        setPaymentVariants(payment);
      })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, []);

  const handleTabChange = (key: InfoTab) => {
    setActiveTab(key);
    navigate(`/info#${key}`, { replace: true });
  };

  useEffect(() => {
  if (hash === '#delivery') setActiveTab('delivery');
  else if (hash === '#contacts') setActiveTab('contacts');
  else if (hash === '#about') setActiveTab('about');
}, [hash]);

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Інформація</span>
        </nav>

        <div className="flex flex-col md:flex-row gap-0">
          <div className="flex md:flex-col overflow-x-auto md:overflow-visible scrollbar-hide shrink-0 md:w-48 border-b md:border-b-0 md:border-r border-gray-200 md:pr-6 mb-6 md:mb-0 gap-1">
            {tabs.map(tab => (
              <button
                key={tab.key}
                onClick={() => handleTabChange(tab.key)}
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
            {isLoading ? (
              <p className="text-sm text-gray-400">Завантаження...</p>
            ) : (
              <>
                {activeTab === 'about' && (
                  aboutContent
                    ? <div dangerouslySetInnerHTML={{ __html: aboutContent }} />
                    : <p className="text-sm text-gray-400">Інформація відсутня</p>
                )}

                {activeTab === 'delivery' && (
                  deliveryVariants.length === 0 && paymentVariants.length === 0 ? (
                    <p className="text-sm text-gray-400">Інформація відсутня</p>
                  ) : (
                    <div className="flex flex-col gap-6">
                      {deliveryVariants.length > 0 && (
                        <div>
                          <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">Доставка</h3>
                          <div className="flex flex-col gap-2">
                            {deliveryVariants.map(v => (
                              <div key={v.id} className="border border-gray-200 rounded-lg px-4 py-3">
                                <p className="text-sm font-medium text-gray-700">{v.name}</p>
                                <p className="text-xs text-gray-400 mt-0.5">{v.description}</p>
                              </div>
                            ))}
                          </div>
                        </div>
                      )}
                      {paymentVariants.length > 0 && (
                        <div>
                          <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">Оплата</h3>
                          <div className="flex flex-col gap-2">
                            {paymentVariants.map(v => (
                              <div key={v.id} className="border border-gray-200 rounded-lg px-4 py-3">
                                <p className="text-sm font-medium text-gray-700">{v.name}</p>
                                <p className="text-xs text-gray-400 mt-0.5">{v.description}</p>
                              </div>
                            ))}
                          </div>
                        </div>
                      )}
                    </div>
                  )
                )}

                {activeTab === 'contacts' && (
                  contactsContent
                    ? <div dangerouslySetInnerHTML={{ __html: contactsContent }} />
                    : <p className="text-sm text-gray-400">Інформація відсутня</p>
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </Layout>
  );
}