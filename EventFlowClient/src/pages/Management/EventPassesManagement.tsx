import { DataTable } from "primereact/datatable";
import { EventPass, Reservation } from "../../models/response_models";
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
import DetailsEventPassDialog from "../../components/profile/eventpasses/DetailsEventPassDialog";
import DownloadEventPassDialog from "../../components/profile/eventpasses/DownloadEventPassDialog";
import CancelEventPassDialog from "../../components/management/eventpasses/CancelEventPassDialog";

const EventPassesManagement = () => {
  const { data: items, get: getItems } = useApi<EventPass>(ApiEndpoint.EventPass);

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
      body: (rowData: EventPass) => rowData.id,
    },
    {
      field: "user.name",
      header: "Użytkownik",
      sortable: true,
      body: (rowData: EventPass) => `${rowData.user?.name} ${rowData.user?.surname}`,
    },
    {
      field: "startDate",
      header: "Data zakupu",
      sortable: true,
      body: (rowData: EventPass) => DateFormatter.FormatDate(rowData.startDate, DateFormat.Date),
    },
    {
      field: "renewalDate",
      header: "Data przedłużenia",
      sortable: true,
      body: (rowData: EventPass) => DateFormatter.FormatDate(rowData.renewalDate, DateFormat.Date),
    },
    {
      field: "endDate",
      header: "Data zakończenia",
      sortable: true,
      body: (rowData: EventPass) => DateFormatter.FormatDate(rowData.endDate, DateFormat.Date),
    },
    {
      field: "passType.name",
      header: "Typ karnetu",
      sortable: true,
      body: (rowData: EventPass) => rowData.passType?.name,
    },
    {
      field: "paymentAmount",
      header: "Cena (zł)",
      sortable: true,
      body: (rowData: Reservation) => rowData.paymentAmount,
    },
    {
      field: "eventPassStatus",
      header: "Status",
      sortable: false,
      body: (rowData: EventPass) => (
        <StatusBody
          status={rowData.eventPassStatus}
          activeStatusText="Aktywny karnet"
          expiredStatusText="Przeterminowany karnet"
          canceledStatusText="Anulowany karnet"
          unknownStatusText="Nieznany status"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: EventPass) => (
        <ActionsTemplate
          rowData={rowData}
          status={rowData.eventPassStatus}
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
  } = useTable<EventPass[], EventPass>(items, cols, "karnety");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CancelEventPassDialog
        ref={deleteDialog}
        eventPass={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogConfirm={reloadItemsAfterSuccessDialog}
      />

      <DetailsEventPassDialog ref={detailsDialog} isAdminDetails={true} eventPass={itemToDetails} />

      <DownloadEventPassDialog
        ref={downloadDialog}
        eventPass={itemToDownload}
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
          "user.name",
          "startDate",
          "renewalDate",
          "endDate",
          "passType.name",
          "paymentAmount",
        ]}
        emptyMessage="Brak karnetów"
        header={
          <HeaderTemplate
            headerText="Karnety"
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
export default EventPassesManagement;
