extends Node

signal cell_selected(cell: Cell, data: TileData)
signal show_building_info(building: Building)
signal add_route(to: Building)
signal add_route_ui(route: TransportRoute)
signal toggle_modal(window_name: String)
