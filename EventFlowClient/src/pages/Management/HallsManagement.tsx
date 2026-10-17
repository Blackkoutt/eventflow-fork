import { DataTable } from "primereact/datatable";
import { Hall } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import DownloadHallViewDialog from "../../components/management/halls/DownloadHallViewDialog";
import DetailsHallDialog from "../../components/management/halls/DetailsHallDialog";
import DeleteHallDialog from "../../components/management/halls/DeleteHallDialog";
import ModifyHallDialog from "../../components/management/halls/ModifyHallDialog";
import CreateHallDialog from "../../components/management/halls/CreateHallDialog";

const HallsManagement = () => {
  const { data: items, get: getItems } = useApi<Hall>(ApiEndpoint.Hall);

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
      body: (rowData: Hall) => rowData.id,
    },
    {
      field: "hallNr",
      header: "Nr sali",
      sortable: true,
      body: (rowData: Hall) => rowData.hallNr,
    },
    {
      field: "floor",
      header: "Piętro",
      sortable: true,
      body: (rowData: Hall) => rowData.floor,
    },
    {
      field: "rentalPricePerHour",
      header: "Cena za 1h (zł)",
      sortable: true,
      body: (rowData: Hall) => rowData.rentalPricePerHour,
    },
    {
      field: "type?.name",
      header: "Typ",
      sortable: true,
      body: (rowData: Hall) => rowData.type?.name,
    },
    {
      field: "seatsCount",
      header: "Ilość miejsc",
      sortable: true,
      body: (rowData: Hall) => rowData.seatsCount,
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: Hall) => (
        <ActionsTemplate
          rowData={rowData}
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
    createDialog,
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
    onCreate,
    onModify,
    closeDialogsAndSetValuesToDefault,
  } = useTable<Hall[], Hall>(items, cols, "sale");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreateHallDialog
        ref={createDialog}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DownloadHallViewDialog
        ref={downloadDialog}
        hall={itemToDownload}
        onDialogClose={onDialogClose}
      />

      <DetailsHallDialog ref={detailsDialog} hall={itemToDetails} />

      <DeleteHallDialog
        ref={deleteDialog}
        hall={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogConfirm={reloadItemsAfterSuccessDialog}
      />

      <ModifyHallDialog
        ref={modifyDialog}
        hallId={itemToModify?.id}
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
        globalFilterFields={["hallNr", "floor", "rentalPricePerHour", "type?.name", "seatsCount"]}
        emptyMessage="Brak sal"
        header={
          <HeaderTemplate
            headerText="Sale"
            onCreate={onCreate}
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
export default HallsManagement;
