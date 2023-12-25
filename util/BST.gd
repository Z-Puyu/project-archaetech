class_name BST extends RefCounted

const BSTVertex = preload("res://util/BSTVertex.gd");

var root: BSTVertex;

func search(T: BSTVertex, value: Variant):
	if (T == null):
		return T;
	elif (T.value == value):
		return T; 
	elif (T.value < value):
		return self.search(T.right, value);
	else:
		return self.search(T.left, value);
		
func insert(T: BSTVertex, value: Variant, other: Variant = null):
	if (T == null):
		return BSTVertex.new(value, other);
		
	if (T.value < value):
		T.right = self.insert(T.right, value, other);
	else:
		T.left = self.insert(T.left, value, other);
	
	return T;
	
func inorder(T: BSTVertex):
	var result: Array[BSTVertex] = []
	if (T == null):
		return null;
	var left = inorder(T.left);
	var right = inorder(T.right);
	if left != null:
		result += left;
	result += [T];
	if right != null:
		result += right;
	return result;
	
func findMin(T: BSTVertex):
	if (T == null):
		return "no result";
	elif (T.left == null):
		return T;
	else:
		return findMin(T.left);

func findMax(T: BSTVertex):
	if (T == null):
		return "no result";
	elif (T.right == null):
		return T;
	else:
		return findMin(T.right);
		
func successor(T: BSTVertex):
	if(T.right != null):
		return findMin(T.right);
	else:
		var par: BSTVertex = T.parent;
		var cur: BSTVertex = T;
		while ((par != null) and (cur == par.right)):
			cur = par;
			par = cur.parent;
		if par == null:
			return -1;
		return par.key;
			
func predecessor(T: BSTVertex):
	if(T.left != null):
		return findMax(T.right);
	else:
		var par: BSTVertex = T.parent;
		var cur: BSTVertex = T;
		while ((par != null) and (cur == par.left)):
			cur = par;
			par = cur.parent;
		if par == null:
			return -1;
		return par.key;

func delete(T: BSTVertex, v: Variant):
	if(T == null):
		return T;
	if(T.value == v):
		if(T.left == null and T.right == null):
			T = null;
		elif(T.left == null and T.right != null):
			T.right.parent = T.parent;          
			T = T.right;
		elif(T.left != null and T.right == null):
			T.left.parent = T.parent;          
			T = T.left;
		else:
			var successor = successor(v)
			T.key = successor;
			T.right = delete(T.right, successor);
	elif(T.value < v):
		T.right = delete(T.right, v);
	else:
		T.left = delete(T.left, v);
	return T;
	
func _init():
	self.root = null;
	
func BSTsearch(v):
	var res: BSTVertex = search(root, v);
	if res != null:
		return res;
	return "no result";

func BSTinsert(v: Variant, other: Variant):
	root = insert(root, v, other);

func BSTinorder():
	return inorder(root);
	
func BSTfindMin():
	return findMin(root);
	
func BSTfindMax():
	return findMax(root);

func BSTsuccessor(v: Variant):
	var vPos: BSTVertex = search(root, v);
	if vPos != null:
		return successor(vPos);
	return "no result"

func BSTpredecessor(v: Variant):
	var vPos: BSTVertex = search(root, v);
	if vPos != null:
		return predecessor(vPos);
	return "no result"	
	
func BSTdelete(v: Variant):
	root = delete(root, v);

func getHeight(T: BSTVertex):
	if(T == null):
		return -1;
	else:
		return max(getHeight(T.left), getHeight(T.right)) + 1

func BSTgetHeight():
	return getHeight(root);

func is_empty():
	return self.root == null;
