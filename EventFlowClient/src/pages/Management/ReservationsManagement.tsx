import { DataTable } from "primereact/datatable";
import { Reservation } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import StatusBody from "../../components/tabledata/StatusBody";
import CancelReservationDialog from "../../components/profile/reservations/CancelReservationDialog";
import DetailsReservationDialog from "../../components/profile/reservations/DetailsReservationDialog";
import DownloadReservationTicketDialog from "../../components/profile/reservations/DownloadReservationTicketDialog";

const ReservationsManagement = () => {
  const { data: items, get: getItems } = useApi<Reservation>(ApiEndpoint.Reservation);

  useEffect(() => {
    getItems({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: Reservation) => rowData.id,
    },
    {
      field: "reservationDate",
      header: "Data rezerwacji",
      sortable: true,
      body: (rowData: Reservation) =>
        DateFormatter.FormatDate(rowData.reservationDate, DateFormat.DateTime),
    },
    {
      field: "user.name",
      header: "Użytkownik",
      sortable: true,
      body: (rowData: Reservation) => `${rowData.user?.name} ${rowData.user?.surname}`,
    },
    {
      field: "ticket.festival.name",
      header: "Festiwal",
      sortable: true,
      body: (rowData: Reservation) => rowData.ticket?.festival?.title,
    },
    {
      field: "ticket.event.name",
      header: "Wydarzenie",
      sortable: true,
      body: (rowData: Reservation) => rowData.ticket?.event?.title,
    },
    {
      field: "paymentAmount",
      header: "Cena (zł)",
      sortable: true,
      body: (rowData: Reservation) => rowData.paymentAmount,
    },
    {
      field: "reservationStatus",
      header: "Status",
      sortable: false,
      body: (rowData: Reservation) => (
        <StatusBody
          status={rowData.reservationStatus}
          activeStatusText="Aktywna rezerwacja"
          canceledStatusText="Anulowana rezerwacja"
          expiredStatusText="Przedawniona rezerwacja"
          unknownStatusText="Nieznany status"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: Reservation) => (
        <ActionsTemplate
          rowData={rowData}
          status={rowData.reservationStatus}
          includeModify={false}
          includeDownload={true}
          onDetails={onDetails}
          onDelete={onDelete}
          onDownload={onDownload}
        />
      ),
    },
  ];

  const {
    dt,
    deleteDialog,
    detailsDialog,
    downloadDialog,
    itemToDelete,
    itemToDownload,
    itemToDetails,
    filters,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDelete,
    onDetails,
    onDownload,
    closeDialogsAndSetValuesToDefault,
  } = useTable<Reservation[], Reservation>(items, cols, "rezerwacje");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CancelReservationDialog
        ref={deleteDialog}
        reservation={itemToDelete}
        isAdminCancel={true}
        onDialogClose={onDialogClose}
        onDialogConfirm={reloadItemsAfterSuccessDialog}
      />

      <DetailsReservationDialog
        isAdminDetails={true}
        ref={detailsDialog}
        reservation={itemToDetails}
      />

      <DownloadReservationTicketDialog
        ref={downloadDialog}
        reservation={itemToDownload}
        onDialogClose={onDialogClose}
      />

      <DataTable
        ref={dt}
        value={items}
        paginator
        removableSort
        filters={filters}
        filterDisplay="row"
        globalFilterFields={[
          "reservationDate",
          "user.name",
          "ticket.festival.name",
          "ticket.event.name",
          "paymentAmount",
        ]}
        emptyMessage="Brak rezerwacji"
        header={
          <HeaderTemplate
            headerText="Rezerwacje"
            includeCreate={false}
            menuElements={menuElements}
            globalFilterValue={globalFilterValue}
            onGlobalFilterChange={(e) => onGlobalFilterChange(e)}
          />
        }
        rows={5}
        style={{ minWidth: "62vw" }}
        rowsPerPageOptions={[5, 10, 25, 50]}
        stripedRows
        showGridlines
      >
        {cols.map((col, index) => (
          <Column
            key={index}
            field={col.field}
            header={col.header}
            body={(rowData) => col.body(rowData)}
            sortable={col.sortable}
          />
        ))}
      </DataTable>
    </div>
  );
};
export default ReservationsManagement;
