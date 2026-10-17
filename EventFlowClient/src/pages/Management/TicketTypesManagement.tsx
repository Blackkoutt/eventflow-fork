import { DataTable } from "primereact/datatable";
import { TicketType } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import CreateTicketTypeDialog from "../../components/management/tickettypes/CreateTicketTypeDialog";
import DeleteTicketTypeDialog from "../../components/management/tickettypes/DeleteTicketTypeDialog";
import ModifyTicketTypeDialog from "../../components/management/tickettypes/ModifyTicketTypeDialog";

const TicketTypesManagement = () => {
  const { data: items, get: getItems } = useApi<TicketType>(ApiEndpoint.TicketType);

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
      body: (rowData: TicketType) => rowData.id,
    },
    {
      field: "name",
      header: "Nazwa",
      sortable: true,
      body: (rowData: TicketType) => rowData.name,
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: TicketType) => (
        <ActionsTemplate
          includeDetails={false}
          rowData={rowData}
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
  } = useTable<TicketType[], TicketType>(items, cols, "typy_biletów");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreateTicketTypeDialog
        ref={createDialog}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DeleteTicketTypeDialog
        ref={deleteDialog}
        maxWidth={550}
        item={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <ModifyTicketTypeDialog
        ref={modifyDialog}
        item={itemToModify}
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
        emptyMessage="Brak typów biletów"
        header={
          <HeaderTemplate
            headerText="Typy biletów"
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
export default TicketTypesManagement;
