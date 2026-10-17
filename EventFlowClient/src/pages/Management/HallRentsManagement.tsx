import { DataTable } from "primereact/datatable";
import {
  AdditionalServices,
  HallRent,
} from "../../models/response_models";
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
import DownloadHallRentDialog from "../../components/profile/hallrents/DownloadHallRentDialog";
import CancelHallRentDialog from "../../components/profile/hallrents/CancelHallRentDialog";
import DetailsHallRentDialog from "../../components/profile/hallrents/DetailsHallRentDialog";
import ModifyHallDialog from "../../components/profile/hallrents/ModifyHallDialog";

const HallRentsManagement = () => {
  const { data: items, get: getItems } = useApi<HallRent>(ApiEndpoint.HallRent);

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
      body: (rowData: HallRent) => rowData.id,
    },
    {
      field: "user.name",
      header: "Użytkownik",
      sortable: true,
      body: (rowData: HallRent) => `${rowData.user?.name} ${rowData.user?.surname}`,
    },
    {
      field: "startDate",
      header: "Data rozpoczęcia",
      sortable: true,
      body: (rowData: HallRent) => DateFormatter.FormatDate(rowData.startDate, DateFormat.DateTime),
    },
    {
      field: "endDate",
      header: "Data zakończenia",
      sortable: true,
      body: (rowData: HallRent) => DateFormatter.FormatDate(rowData.endDate, DateFormat.DateTime),
    },
    {
      field: "hall.hallNr",
      header: "Nr sali",
      sortable: true,
      body: (rowData: HallRent) => rowData.hall?.hallNr,
    },
    {
      field: "hall.rentalPricePerHour",
      header: "Cena za 1h (zł)",
      sortable: true,
      body: (rowData: HallRent) => rowData.hall?.rentalPricePerHour,
    },
    {
      field: "additionalServices",
      header: "Dodatkowe usługi",
      sortable: false,
      body: (rowData: HallRent) =>
        rowData.additionalServices.map((service: AdditionalServices) => service.name).join(", "),
    },
    {
      field: "paymentAmount",
      header: "Koszt (zł)",
      sortable: true,
      body: (rowData: HallRent) => rowData.paymentAmount,
    },
    {
      field: "hallRentStatus",
      header: "Status",
      sortable: false,
      body: (rowData: HallRent) => (
        <StatusBody
          status={rowData.hallRentStatus}
          activeStatusText="Aktywna rezerwacja sali"
          expiredStatusText="Zakończona rezerwacja sali"
          canceledStatusText="Anulowana rezerwacja sali"
          unknownStatusText="Nieznany status"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: HallRent) => (
        <ActionsTemplate
          rowData={rowData}
          status={rowData.hallRentStatus}
          includeDownload={true}
          onDetails={onDetails}
          onDelete={onDelete}
          onModify={onModify}
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
    modifyDialog,
    itemToDelete,
    itemToDownload,
    itemToDetails,
    itemToModify,
    filters,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDelete,
    onDetails,
    onDownload,
    onModify,
    closeDialogsAndSetValuesToDefault,
  } = useTable<HallRent[], HallRent>(items, cols, "wynajmy_sal");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <DownloadHallRentDialog
        ref={downloadDialog}
        hallRent={itemToDownload}
        onDialogClose={onDialogClose}
      />

      <ModifyHallDialog
        ref={modifyDialog}
        hallId={itemToModify?.hall?.id}
        hallRentId={itemToModify?.id}
        onDialogClose={onDialogClose}
        onDialogConfirm={reloadItemsAfterSuccessDialog}
      />

      <DetailsHallRentDialog ref={detailsDialog} hallRent={itemToDetails} isAdminDetails={true} />

      <CancelHallRentDialog
        ref={deleteDialog}
        hallRent={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogConfirm={reloadItemsAfterSuccessDialog}
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
          "endDate",
          "hall.hallNr",
          "hall.rentalPricePerHour",
          "paymentAmount",
        ]}
        emptyMessage="Brak rezerwacji sal"
        header={
          <HeaderTemplate
            headerText="Wynajmy sal"
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
export default HallRentsManagement;
