using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class RootNode
{
	Node root;
	#region(Constructor)
	public RootNode(string path)
	{
		this.root = new Node(path, Node.Root);

		Sync();
	}
	#endregion
	#region(Methods)
	private void Sync() {

		try
		{
			// sync folders
			string[] dirs = Directory.GetDirectories(this.root.Path, "*", SearchOption.AllDirectories);
			foreach (string dir in dirs)
			{
				DirectoryInfo dirInfo = new DirectoryInfo(dir);
				Add(dirInfo.Parent.FullName, dirInfo.Name, Node.Folder);
			}

			// sync items
			string[] items = Directory.GetFiles(this.root.Path, "*", SearchOption.AllDirectories);
			foreach (string item in items)
			{
				DirectoryInfo itemInfo = new DirectoryInfo(item);
				Add(itemInfo.Parent.FullName, itemInfo.Name, Node.Item);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("caution: unable to sync items", ex.Message);
		}
	}

	public Node Add(string p, string name, int type) {

		if (type == Node.Root) {
			throw new Exception("cannot add more than one root nodes");
		}

		if (!p.StartsWith(this.root.Path))
		{
			string sep = "\\";
			if (p.StartsWith(sep))
			{
				p = this.root.Path + p;
			} else
            {
				p = this.root.Path + "\\" + p;
			}
		}

		// at this point p will always be a absolute path

		Node node = getNodeObjRef(p);

		if (node == null) {
			throw new Exception("error: Node not found or not compatiable");
		}

		Node newNode = node.Add(name, type);
		return newNode;
	}

	public Node Delete(string p)
	{
		Node node = getNodeObjRef(p);

		if (node == null) {
			throw new Exception("error: unable to find the node for deletion");
		}

		Node parentNode = getNodeObjRef(node.ParentPath);

		if (parentNode == null)
		{
			throw new Exception("error: unable to find the parent node for deletion");
		}

		node.Delete();

		// sync with in memory model
		parentNode.List.Remove(node);

		return node;
	}

	public Node Move(string sourcePath, string destPath)
	{
		if (sourcePath == destPath)
		{
			throw new Exception("error: source path and destination path cannot be the same");
		}

		Node sourceNode = getNodeObjRef(sourcePath);

		if (sourceNode == null)
		{
			throw new Exception("error: unable to find the source node");
		}

		Node sourceParentNode = getNodeObjRef(sourceNode.ParentPath);

		if (sourceParentNode == null)
		{
			throw new Exception("error: unable to find the source parent node");
		}

		Node destNode = getNodeObjRef(destPath);
		if (destNode == null)
		{
			throw new Exception("error: unable to find the destination node");
		}
		if (destNode.IsItem())
		{
			throw new Exception("error: destination directory should be a folder");
		}

		Directory.Move(sourcePath, destPath + "\\" + sourceNode.Name);

		sourceNode.Move(destPath);

		destNode.List.Add(sourceNode);
		sourceParentNode.List.Remove(sourceNode);

		return sourceNode;
	}

	public int Search(string search)
	{
		List<Node> results = new List<Node>();

		SearchInTree(this.root, search, results);

		return results.Count;
	}

	public Node getNodeObjRef(string p) {
		return findNodeObjRef(this.root, p);
	}

	public void Display() {

		PrintTree(this.root);
	}

	private void PrintTree(Node obj)
	{
		Console.WriteLine(obj.GetTypeDisplay() + " " + obj.Path);

		foreach (Node node in obj.List)
		{
			PrintTree(node);
		}
	}

	private void SearchInTree(Node obj, string search, List<Node> results)
	{
		if (search.Contains("*") || search.Contains("?"))
		{
			Regex rx = new Regex(search, RegexOptions.IgnoreCase);
			if (rx.IsMatch(obj.Name))
			{
				Console.WriteLine(obj.GetTypeDisplay() + " " + obj.Name);
				results.Add(obj);
			}
		}
		else if (obj.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
		{
			Console.WriteLine(obj.GetTypeDisplay() + " " + obj.Name);
			results.Add(obj);
		}

		foreach (Node node in obj.List)
		{
			SearchInTree(node, search, results);
		}
	}

	private Node findNodeObjRef(Node obj, string abspath)
	{
		if (obj.Path == abspath)
		{
			return obj;
		}

		foreach (Node node in obj.List)
		{
			Node res = findNodeObjRef(node, abspath);
			if (res != null)
			{
				return res;
			}
		}

		return null;
	}
    #endregion
}
