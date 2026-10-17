import { createBrowserRouter } from "react-router-dom";
import HomePage from "../pages/HomePage";
import NotFoundPage from "../pages/NotFoundPage";
import AboutPage from "../pages/AboutPage";
import ArchivePage from "../pages/ArchivePage";
import EventsPage from "../pages/EventsPage";
import EventPassesPage from "../pages/EventPassesPage";
import FestivalsPage from "../pages/FestivalsPage";
import NewsPage from "../pages/NewsPage";
import RentsPage from "../pages/RentsPage";
import AppLayout from "../layout/AppLayout";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";
import ProtectedRoute from "../wrappers/ProtectedRoute";
import { Roles } from "../helpers/enums/UserRoleEnum";
import UserInfo from "../pages/Profile/UserInfo";
import UserAdditionalInfo from "../pages/Profile/UserAdditionalInfo";
import UserReservations from "../pages/Profile/UserReservations";
import UserEventPasses from "../pages/Profile/UserEventPasses";
import UserHallRents from "../pages/Profile/UserHallRents";
import HomeManagement from "../pages/Management/HomeManagement";
import AdditionalServicesManagement from "../pages/Management/AdditionalServicesManagement";
import HallEquipmentsManagement from "../pages/Management/HallEquipmentsManagement";
import EventCategoriesManagement from "../pages/Management/EventCategoriesManagement";
import EventPassesManagement from "../pages/Management/EventPassesManagement";
import EventPassTypesManagement from "../pages/Management/EventPassTypesManagement";
import FestivalsManagement from "../pages/Management/FestivalsManagement";
import FaqManagement from "../pages/Management/FaqManagement";
import HallRentsManagement from "../pages/Management/HallRentsManagement";
import HallsManagement from "../pages/Management/HallsManagement";
import HallTypesManagement from "../pages/Management/HallTypesManagement";
import MediaPatronsManagement from "../pages/Management/MediaPatronsManagement";
import NewsManagement from "../pages/Management/NewsManagement";
import PartnersManagement from "../pages/Management/PartnersManagement";
import ReservationsManagement from "../pages/Management/ReservationsManagement";
import SeatTypesManagement from "../pages/Management/SeatTypesManagement";
import SponsorsManagement from "../pages/Management/SponsorsManagement";
import StatsManagement from "../pages/Management/StatsManagement";
import TicketTypesManagement from "../pages/Management/TicketTypesManagement";
import UsersManagement from "../pages/Management/UsersManagement";
import OrganizersManagement from "../pages/Management/OrganizersManagement";
import EventsManagement from "../pages/Management/EventsManagement";
import EventPage from "../pages/EventPage";
import FestivalPage from "../pages/FestivalPage";
import ConcreteNews from "../pages/ConcreteNewsPage";
import FAQPage from "../pages/FAQPage";
import AccessibilityDeclarationPage from "../pages/AccessibilityDeclarationPage";
import PrivacyPolicyPage from "../pages/PrivacyPolicyPage";
import RodoPage from "../pages/RodoPage";
import StatutePage from "../pages/StatutePage";
import UserProfilePage from "../pages/UserProfilePage";
import ManagementPage from "../pages/ManagementPage";
import RentPage from "../pages/RentPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <AppLayout />,
    errorElement: <NotFoundPage />,
    children: [
      {
        path: "/",
        element: <HomePage />,
      },
      {
        path: "/about",
        element: <AboutPage />,
      },
      {
        path: "/archive",
        element: <ArchivePage />,
      },
      {
        path: "/accessibility-declaration",
        element: <AccessibilityDeclarationPage />,
      },
      {
        path: "/privacy-policy",
        element: <PrivacyPolicyPage />,
      },
      {
        path: "/rodo",
        element: <RodoPage />,
      },
      {
        path: "/statute",
        element: <StatutePage />,
      },
      {
        path: "/faqs",
        element: <FAQPage />,
      },
      {
        path: "/events",
        element: <EventsPage />,
      },
      {
        path: "/events/:eventId",
        element: <EventPage />,
      },
      {
        path: "/eventpasses",
        element: <EventPassesPage />,
      },
      {
        path: "/festivals",
        element: <FestivalsPage />,
      },
      {
        path: "/festivals/:festivalId",
        element: <FestivalPage />,
      },
      {
        path: "/news",
        element: <NewsPage />,
      },
      {
        path: "/news/:newsId",
        element: <ConcreteNews />,
      },
      {
        path: "/rents",
        element: <RentsPage />,
      },
      {
        path: "/rents/:hallId",
        element: <RentPage />,
      },
      {
        path: "/sign-in",
        element: <LoginPage />,
      },
      {
        path: "/sign-up",
        element: <RegisterPage />,
      },
      {
        path: "/profile",
        element: (
          <ProtectedRoute>
            <UserProfilePage />
          </ProtectedRoute>
        ),
        children: [
          {
            path: "/profile",
            element: (
              <ProtectedRoute>
                <UserInfo />
              </ProtectedRoute>
            ),
          },
          {
            path: "/profile/info",
            element: (
              <ProtectedRoute>
                <UserAdditionalInfo />
              </ProtectedRoute>
            ),
          },
          {
            path: "/profile/reservations",
            element: (
              <ProtectedRoute>
                <UserReservations />
              </ProtectedRoute>
            ),
          },
          {
            path: "/profile/eventpasses",
            element: (
              <ProtectedRoute>
                <UserEventPasses />
              </ProtectedRoute>
            ),
          },
          {
            path: "/profile/hallrents",
            element: (
              <ProtectedRoute>
                <UserHallRents />
              </ProtectedRoute>
            ),
          },
        ],
      },
      {
        path: "/management",
        element: (
          <ProtectedRoute allowedRoles={[Roles.Admin]}>
            <ManagementPage />
          </ProtectedRoute>
        ),
        children: [
          {
            path: "/management",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <HomeManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/additional-services",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <AdditionalServicesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/hall-equipments",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <HallEquipmentsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/event-categories",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <EventCategoriesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/event-passes",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <EventPassesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/event-pass-types",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <EventPassTypesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/events",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <EventsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/festivals",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <FestivalsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/faq",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <FaqManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/hall-rents",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <HallRentsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/halls",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <HallsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/hall-types",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <HallTypesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/media-patrons",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <MediaPatronsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/news",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <NewsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/organizers",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <OrganizersManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/partners",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <PartnersManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/reservations",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <ReservationsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/seat-types",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <SeatTypesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/sponsors",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <SponsorsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/stats",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <StatsManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/ticket-types",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <TicketTypesManagement />
              </ProtectedRoute>
            ),
          },
          {
            path: "/management/users",
            element: (
              <ProtectedRoute allowedRoles={[Roles.Admin]}>
                <UsersManagement />
              </ProtectedRoute>
            ),
          },
        ],
      },
    ],
  },
]);
