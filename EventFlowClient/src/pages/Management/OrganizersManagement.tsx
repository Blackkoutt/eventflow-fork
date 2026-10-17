import { DataTable } from "primereact/datatable";
import { Organizer } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import CreateOrganizerDialog from "../../components/management/organizers/CreateOrganizerDialog";
import DeleteOrganizerDialog from "../../components/management/organizers/DeleteOrganizerDialog";
import ModifyOrganizerDialog from "../../components/management/organizers/ModifyOrganizerDialog";
import ApiClient from "../../services/api/ApiClientService";

const OrganizersManagement = () => {
  const { data: items, get: getItems } = useApi<Organizer>(ApiEndpoint.Organizer);

  useEffect(() => {
    getItems({ id: undefined, queryParams: { kind: "Organizer" } });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: Organizer) => rowData.id,
    },
    {
      field: "name",
      header: "Nazwa",
      sortable: true,
      body: (rowData: Organizer) => rowData.name,
    },
    {
      field: "photoEndpoint",
      header: "Zdjęcie",
      body: (rowData: Organizer) => (
        <img
          src={`${ApiClient.GetPhotoEndpoint(rowData.photoEndpoint)}`}
          alt={`Zdjęcie organizatora o id ${rowData.id}`}
          className="w-[100px] h-[100px] object-contain"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: Organizer) => (
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
  } = useTable<Organizer[], Organizer>(items, cols, "organizatorzy");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: { kind: "Organizer" } });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreateOrganizerDialog
        minWidth={300}
        maxWidth={600}
        ref={createDialog}
        onDialogClose={onDialogClose}
        paddingX={32}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DeleteOrganizerDialog
        ref={deleteDialog}
        maxWidth={750}
        item={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <ModifyOrganizerDialog
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
        emptyMessage="Brak organizatorów"
        header={
          <HeaderTemplate
            headerText="Organizatorzy"
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
export default OrganizersManagement;
