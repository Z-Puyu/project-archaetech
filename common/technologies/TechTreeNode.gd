class_name TechTreeNode extends TextureButton

var _children: Array[TechTreeNode]
var _parents: Array[TechTreeNode]

func children() -> Array[TechTreeNode]:
	return self._children
	
func prerequisites() -> Array[TechTreeNode]:
	return self._parents
