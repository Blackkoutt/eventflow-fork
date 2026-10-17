import { DataTable } from "primereact/datatable";
import { MediaPatron } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import CreateMediaPatronDialog from "../../components/management/mediapatrons/CreateMediaPatronDialog";
import DeleteMediaPatronDialog from "../../components/management/mediapatrons/DeleteMediaPatronDialog";
import ModifyMediaPatronDialog from "../../components/management/mediapatrons/ModifyMediaPatronDialog";
import ApiClient from "../../services/api/ApiClientService";

const MediaPatronsManagement = () => {
  const { data: items, get: getItems } = useApi<MediaPatron>(ApiEndpoint.MediaPatron);

  useEffect(() => {
    getItems({ id: undefined, queryParams: { kind: "MediaPatron" } });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: MediaPatron) => rowData.id,
    },
    {
      field: "name",
      header: "Nazwa",
      sortable: true,
      body: (rowData: MediaPatron) => rowData.name,
    },
    {
      field: "photoEndpoint",
      header: "Zdjęcie",
      body: (rowData: MediaPatron) => (
        <img
          src={`${ApiClient.GetPhotoEndpoint(rowData.photoEndpoint)}`}
          alt={`Zdjęcie patrona medialnego o id ${rowData.id}`}
          className="w-[100px] h-[100px] object-contain"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: MediaPatron) => (
        <ActionsTemplate
          rowData={rowData}
          includeDetails={false}
          onModify={onModify}
          onDelete={onDelete}
        />
      ),
    },
  ];

  const {
    dt,
    deleteDialog,
    createDialog,
    modifyDialog,
    itemToDelete,
    itemToModify,
    filters,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDelete,
    onModify,
    onCreate,
    closeDialogsAndSetValuesToDefault,
  } = useTable<MediaPatron[], MediaPatron>(items, cols, "patroni_medialni");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: { kind: "MediaPatron" } });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreateMediaPatronDialog
        minWidth={300}
        maxWidth={600}
        ref={createDialog}
        onDialogClose={onDialogClose}
        paddingX={32}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DeleteMediaPatronDialog
        ref={deleteDialog}
        maxWidth={750}
        item={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <ModifyMediaPatronDialog
        ref={modifyDialog}
        item={itemToModify}
        minWidth={400}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DataTable
        ref={dt}
        value={items}
        paginator
        removableSort
        filters={filters}
        filterDisplay="row"
        globalFilterFields={["name"]}
        emptyMessage="Brak patronów medialnych"
        header={
          <HeaderTemplate
            headerText="Patroni medialni"
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
export default MediaPatronsManagement;
