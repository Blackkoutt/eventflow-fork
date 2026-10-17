import { useState, useEffect, useRef } from "react";
import { DataTable, DataTableFilterMeta, DataTableValueArray } from "primereact/datatable";
import { ExportColumns } from "../helpers/FileExporters";
import FileExporter from "../helpers/FileExporters";
import { ButtonWithMenuElement } from "../components/buttons/ButtonWithMenu";
import { faFile, faFileExcel, faFilePdf } from "@fortawesome/free-solid-svg-icons";
import { FilterMatchMode } from "primereact/api";
import { useDialogs } from "./useDialogs";

export const useTable = <TData extends DataTableValueArray, TEntity>(
  data: TData,
  columns: any[],
  exportFileName: string
) => {
  const dt = useRef<DataTable<TData>>(null);

  const [filters, setFilters] = useState<DataTableFilterMeta>({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  });
  const [globalFilterValue, setGlobalFilterValue] = useState<string>("");

  const [exportColumns, setExportColumns] = useState<ExportColumns[]>([]);

  useEffect(() => {
    setExportColumns(
      columns.map((col) => ({
        title: col.header,
        dataKey: col.field,
        body: col.body,
      }))
    );
  }, []);

  const onGlobalFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    let _filters = { ...filters };

    // @ts-ignore
    _filters["global"].value = value;

    setFilters(_filters);
    setGlobalFilterValue(value);
  };

  const menuElements: ButtonWithMenuElement[] = [
    {
      icon: faFile,
      text: "CSV",
      iconColor: "#06b6d4",
      action: () => FileExporter.ExportAsCSV(dt.current),
    },
    {
      icon: faFileExcel,
      text: "XLS",
      iconColor: "#22c55e",
      action: () => FileExporter.ExportAsXLSX(data, exportFileName),
    },
    {
      icon: faFilePdf,
      text: "PDF",
      iconColor: "#f97316",
      action: () => FileExporter.ExportAsPdf(exportColumns, data, exportFileName),
    },
  ];

  const {
    deleteDialog,
    createDialog,
    detailsDialog,
    modifyDialog,
    downloadDialog,
    itemToDelete,
    itemToDetails,
    itemToModify,
    itemToDownload,
    onDialogClose,
    onDelete,
    onModify,
    onCreate,
    onDetails,
    onDownload,
    closeDialogsAndSetValuesToDefault,
  } = useDialogs<TEntity>();

  return {
    dt,
    deleteDialog,
    createDialog,
    detailsDialog,
    modifyDialog,
    downloadDialog,
    itemToDelete,
    itemToDetails,
    itemToModify,
    itemToDownload,
    filters,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDelete,
    onModify,
    onCreate,
    onDetails,
    onDownload,
    closeDialogsAndSetValuesToDefault,
  };
};
